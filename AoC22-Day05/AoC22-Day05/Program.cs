// See https://aka.ms/new-console-template for more information

using System.Collections;
using System.Linq;

// var path = Path.Combine(Directory.GetCurrentDirectory(), "BasicInput.csv");
var path = Path.Combine(Directory.GetCurrentDirectory(), "AoC22-Day05-input.csv");
var file = File.ReadAllLines(path);

var listOfBlocks = new ArrayList();

// This is rastered - ideally need to know how tall the columns are first - or just go with negatives? Let's go with negatives.
var column = 1;
var row = 1;
var rowWhereInstructionsBegin = 0;
var blocks = true;
var bottomRow = 0;
var numberOfColumns = 9;

foreach (var line in file)
{
    if (line == "")
    {
        rowWhereInstructionsBegin = column+1;
        blocks = false;
        continue;
    }

    if (blocks)
    {
        bottomRow++;
        SetBlock(line);
    }
    else
    {
        ReadInstructions(line);
    }
}


void SetBlock(string line)
{
    var index = 0;

    while (index <= line.Length)
    {
        // Grab the characters between index and index+2
        var potentialBlock = line.Substring(index, 3);
        if (potentialBlock.StartsWith("[") && potentialBlock.EndsWith("]"))
        {
            var newBlock = new Block()
            {
                RowNumber = row,
                ColNumber = column,
                Value = potentialBlock.Substring(1, 1)
            };
            listOfBlocks.Add(newBlock);
        }

        column++;
        index += 4;
    }

    column = 1;
    row++;
}


void ReadInstructions(string line)
{
    FullList();

    var lineInstructions = line.Split(" ");
    var moveAmount = int.Parse(lineInstructions[1]) ;
    var colToMoveFrom = int.Parse(lineInstructions[3]);
    var colToMoveTo = int.Parse(lineInstructions[5]);

    Console.WriteLine($"Instructions: Move amount: {moveAmount} | colToMoveFrom: {colToMoveFrom} | colToMoveTo: {colToMoveTo}");
    

    for (var i = 0; i < moveAmount; i++)
    {
        Console.WriteLine($"Move Amount: {i}");

        Console.WriteLine($"ColToMoveFrom: {colToMoveFrom}");
        // Find the columnToMoveFrom and the lowest row value
        var blocksInColumnToMove = from Block b in listOfBlocks
                                        where b.ColNumber == colToMoveFrom
                                        orderby b.RowNumber ascending 
                                        select b;
        var blockToMove = blocksInColumnToMove.First();
        Console.WriteLine($"Moving {blockToMove}");
        // Find the columnToMoveTo and the lowest value
        var blocksInColumnToAddTo = from Block b in listOfBlocks
                                        where b.ColNumber == colToMoveTo
                                        orderby b.RowNumber ascending
                                        select b;

        // -1 to whatever that value is and change the block's details.
        var lowestRowValue = !blocksInColumnToAddTo.Any() ? bottomRow : blocksInColumnToAddTo.First().RowNumber - 1;

        blockToMove.ColNumber = colToMoveTo;
        blockToMove.RowNumber = lowestRowValue;
        Console.WriteLine($"Have moved {blockToMove}");
    }
}


Console.WriteLine("***");
foreach (var block in listOfBlocks)
{
    Console.WriteLine(block);
}

Console.WriteLine($"Row to start for instructions: {rowWhereInstructionsBegin}");


// Final order - start by ordering by column, then for each column order by ascending row, then take first of each

var finalOrder = from Block b in listOfBlocks
                                    orderby b.ColNumber ascending, b.RowNumber ascending 
                                    select b;

Console.WriteLine("Sorted by Column and Row");


foreach (var block in finalOrder)
{
    Console.WriteLine(block);
}

Console.WriteLine("Final results");
// We know there are 3 columns, so:
var finalOutcome = "";
for (int i = 1; i <= numberOfColumns; i++)
{
    var blocksOfThatColumn = from Block b in finalOrder where b.ColNumber == i select b;
    finalOutcome += blocksOfThatColumn.First();
}

Console.WriteLine($"Final outcome: {finalOutcome}");


void FullList()
{
    Console.WriteLine("Full list:");
    foreach (var block in listOfBlocks)
    {
        Console.WriteLine(block);
    }
}
internal class Block
{
    public int RowNumber
    { get; set; }
    public int ColNumber
    { get; set; }
    public string? Value
    { get; set; }

    public override string ToString()
    {
        return $"{Value}";
    }
}

