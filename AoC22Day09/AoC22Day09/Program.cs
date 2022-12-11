// Data

using System.Data;

var path = Path.Join(Directory.GetCurrentDirectory(), "AoC22Day09Input.csv");
var file = File.ReadAllLines(path);

var listOfHeadPositions = new List<Position>();
var listOfTailPositions = new List<Position>();

var headPosition = new Position() { row = 0, col = 0 };
var tailPosition = new Position() { row = 0, col = 0 };

listOfHeadPositions.Add(headPosition);
listOfTailPositions.Add(tailPosition);

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
        var originalHeadPosition = headPosition;
        // Add that to the head position and store it
        headPosition = new Position()
            { row = headPosition.row + instructionPosition.row, col = headPosition.col + instructionPosition.col };
        Console.WriteLine($"Head: {headPosition}");
        // Add new position to list of HeadPositions
        listOfHeadPositions.Add(headPosition);

        // Work out wtf the tail has to do
        tailPosition = TailPositionAdjustment(headPosition, tailPosition, originalHeadPosition);
        Console.WriteLine($"Tail: {tailPosition}");
        // Add new position to list of TailPositions
        listOfTailPositions.Add(tailPosition);
    }
}

Console.WriteLine("*****");

// https://stackoverflow.com/questions/4991728/how-to-get-a-distinct-list-from-a-list-of-objects
var listOfUniqueTailPositions = listOfTailPositions.GroupBy(elem => new { elem.row, elem.col })
    .Select(group => group.First());

Console.WriteLine(listOfUniqueTailPositions.Count());


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
    Console.WriteLine($"Diff: {difference}");
    // Close enough, within 1 square on row and col
    if (difference.row is >= -1 and <= 1 && difference.col is >= -1 and <= 1)
    {
        Console.WriteLine("> 1. don't move");
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
                Console.WriteLine("2. Move right");
                return new Position { row = tail.row, col=tail.col+1};
            }
            case <= -2:
            {
                Console.WriteLine("3. Move left");
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
                Console.WriteLine("4. Move down");
                return new Position { row = tail.row + 1, col = tail.col };
            }
            case <= -2:
            {
                Console.WriteLine("5. Move up");
                return new Position() { row = tail.row - 1, col = tail.col };
            }
        }
    }
    // Case: Row is 2+ apart, col is 2+ apart
    // Tail moves to previous head
        Console.WriteLine("6. Go to original head");
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