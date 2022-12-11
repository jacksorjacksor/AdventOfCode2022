// Data

using System.Data;
// Get the data
var path = Path.Join(Directory.GetCurrentDirectory(), "AoC22Day09Sample1.csv");
var file = File.ReadAllLines(path);

// Make storage lists - this should probably be list of lists?
var listOfPositions = new List<List<Position>>();

// listOfPositions is a list of all 10 points, and stores their movements.
// This can be used for "originalHeadPosition" values

for (int i = 0; i < 10; i++)
{
    listOfPositions.Add(new List<Position>() { new Position() { row = 0, col = 0 } });
}

var moveCounter = 0;

foreach (var line in file)
{
    var splitLine = line.Split();
    var instruction = new Instruction() { direction = splitLine[0], numberOfMoves =int.Parse(splitLine[1]) };
    Console.WriteLine("*");
    Console.WriteLine($"Instruction: {instruction}");
    for (var i = 0; i < instruction.numberOfMoves; i++)
    {
        Console.WriteLine("-");
        // Parse the direction needed
        var instructionPosition = InstructionMovement(instruction.direction);

        var originalHeadPosition = listOfPositions[0][moveCounter];
        // Add that to the head position and store it
        var newHeadPosition = new Position()
            { row = originalHeadPosition.row + instructionPosition.row, col = originalHeadPosition.col + instructionPosition.col };
        // Console.WriteLine($"Head: {originalHeadPosition}");
        // Add new position to list of HeadPositions
        listOfPositions[0].Add(newHeadPosition);

        for (int j = 1; j < 10; j++)
        {
            var loopNewHeadPosition = listOfPositions[j - 1][moveCounter + 1];
            var loopOriginalTailPosition = listOfPositions[j][moveCounter];
            var loopOriginalHeadPosition = listOfPositions[j - 1][moveCounter];


            // Work out wtf the tail has to do
            var newTailPosition = TailPositionAdjustment(loopNewHeadPosition, loopOriginalTailPosition, loopOriginalHeadPosition);


            // Console.WriteLine($"Tail: {newTailPosition}");
            // Add new position to list of TailPositions
            listOfPositions[j].Add(newTailPosition);
        }

        moveCounter++;

        Console.WriteLine("Current State:");

        for (int k = 0; k < 10; k++)
        {
            var index = k.Equals(0) ? "H" : k.ToString();
            Console.WriteLine($"{index}: {listOfPositions[k][moveCounter]}");
        }
    }


}

Console.WriteLine("*****");

// https://stackoverflow.com/questions/4991728/how-to-get-a-distinct-list-from-a-list-of-objects
var listOfUniqueTailPositions = listOfPositions[10-1].GroupBy(elem => new { elem.row, elem.col })
    .Select(group => group.First());

Console.WriteLine(listOfUniqueTailPositions.Count());
// 4842 - too high

/*foreach (var item in listOfUniqueTailPositions)
{
    Console.WriteLine(item);
}*/

// Console.WriteLine(listOfUniqueTailPositions.Count());
// 11442 (too high - no testing at all first!)
// 5000 (too low - just guessed for fun!)
// 9651 (too high)
// 6357 (correct!)
Position InstructionMovement(string direction)
{
    int rowVal;
    int colVal;
    switch (direction)
    {
        case "U":
            rowVal = -1;
            colVal = 0;
            break;
        case "D":
            rowVal = 1;
            colVal = 0;
            break;
        case "L":
            rowVal = 0;
            colVal = -1;
            break;
        default: // "r"
            rowVal = 0;
            colVal = 1;
            break;
    }
    return new Position() { row = rowVal, col = colVal };
}



Position TailPositionAdjustment(Position head, Position tail, Position originalHeadPosition)
{
    var difference = new Position { row = head.row - tail.row, col = head.col - tail.col };
    // Console.WriteLine($"Diff: {difference}");
    // Close enough, within 1 square on row and col
    if (difference.row is >= -1 and <= 1 && difference.col is >= -1 and <= 1)
    {
        // Console.WriteLine("> 1. don't move");
        // Don't need to move
        return tail;
    }

    // Case: Same row, col is 2+ apart
    if (difference.row.Equals(0) && difference.col is >= 2 or <= -2)
    {
        // Tail keeps same row, changes col
        switch (difference.col)
        {
            case >= 2:
            {
                // Console.WriteLine("2. Move right");
                return new Position { row = tail.row, col=tail.col+1};
            }
            case <= -2:
            {
                // Console.WriteLine("3. Move left");
                return new Position { row = tail.row, col = tail.col - 1 };
            }
        }
    }
    // Case: Same col, row is 2+ apart
    if (difference.row is >= 2 or <= -2 && difference.col.Equals(0))
    {
        // Tail keeps same col, changes row
        switch (difference.row)
        {
            case >= 2:
            {
                // Console.WriteLine("4. Move down");
                return new Position { row = tail.row + 1, col = tail.col };
            }
            case <= -2:
            {
                // Console.WriteLine("5. Move up");
                return new Position() { row = tail.row - 1, col = tail.col };
            }
        }
    }
    // Case: Row is 2+ apart, col is 2+ apart
    // Tail moves to previous head
        // Console.WriteLine("6. Go to original head");
        return originalHeadPosition;
    }


class Position
{
    public int row { get; set; }
    public int col { get; set; }
    public override string ToString()
    {
        return $"R{row}|C{col}";
    }
}

class Instruction
{
    public string direction { get; set; }
    public int numberOfMoves{ get; set; }
    public override string ToString()
    {
        return $"Moves: {numberOfMoves} | Direction: {direction}";
    }
}