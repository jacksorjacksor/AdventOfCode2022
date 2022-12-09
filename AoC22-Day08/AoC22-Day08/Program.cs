// See https://aka.ms/new-console-template for more information

using System.Diagnostics.Metrics;

var path = Path.Join(Directory.GetCurrentDirectory(), "AoC22-Day08-Input.csv");
var file = File.ReadAllLines(path);
/*
30373
25512
65332
33549
35390

Each tree is represented as a single digit whose value is its height, where 0 is the shortest and 9 is the tallest.

A tree is visible if all of the other trees between it and an edge of the grid are shorter than it. 
Only consider trees in the same row or column; that is, only look up, down, left, or right from any given tree.

All of the trees around the edge of the grid are visible 
- since they are already on the edge, there are no trees to block the view. 

In this example, that only leaves the interior nine trees to consider:

    The top-left 5 is visible from the left and top. (It isn't visible from the right or bottom since other trees of height 5 are in the way.)
    The top-middle 5 is visible from the top and right.
    The top-right 1 is not visible from any direction; for it to be visible, there would need to only be trees of height 0 between it and an edge.
    The left-middle 5 is visible, but only from the right.
    The center 3 is not visible from any direction; for it to be visible, there would need to be only trees of at most height 2 between it and an edge.
    The right-middle 3 is visible from the right.
    In the bottom row, the middle 5 is visible, but the 3 and 4 are not.

With 16 trees visible on the edge and another 5 visible in the interior, a total of 21 trees are visible in this arrangement.
*/

// RC crawling!

// Get size of thing
var rowSize = 0;
var colSize = 0;
foreach (var line in file)
{
    if (rowSize.Equals(0))
    {
        rowSize = line.Length;
    }
    colSize++;
}

Console.WriteLine($"Row: {rowSize}");
Console.WriteLine($"Col: {colSize}");

// Go through each value and check whether a lower value is present from T, R, B, L

// Store as List of Lists;

var forestList = new List<List<double>>();

foreach (var line in file)
{
    var forestRow = new List<double>();
    for (var i = 0; i < rowSize; i++)
    {
        var val = line[i];
        forestRow.Add(char.GetNumericValue(line[i]));
    }
    forestList.Add(forestRow);
}


// Checking to see trees
var counterOfVisible = 0;
for (var r = 0; r < rowSize; r++)
{
    var row = forestList[r];
    for (var c = 0; c < colSize; c++)
    {
        var target = forestList[r][c];
        bool spottedTop = CheckTop(r,c,target);
        bool spottedBottom = CheckBottom(r, c, target);
        bool spottedRight = CheckRight(r,c,target);
        bool spottedLeft = CheckLeft(r, c, target);
        if (spottedTop || spottedBottom || spottedRight || spottedLeft)
        {
            Console.WriteLine($"R{r}C{c} ({target})");
            /*
            Console.WriteLine("Spotted!");
            */
            counterOfVisible++;
        }
        /*
        Console.WriteLine("****");
    */
    }
}

Console.WriteLine($"Visible: {counterOfVisible}");
// 669 - too low
bool CheckTop(int row, int col, double targetSquare)
{
    /*
    Console.WriteLine(">> CHECKS <<");
    Console.WriteLine($"Target Square: R{row}C{col} ({targetSquare})");
    */
    double obscuringTreeValue = 0;
    double previousTreeValue = -1;
    for (int i = 0; i < row; i++)
    {
        obscuringTreeValue = forestList[i][col];
        if (targetSquare<= obscuringTreeValue || obscuringTreeValue<previousTreeValue)
        {
            return false;
        }
        previousTreeValue = obscuringTreeValue;
    }
    return true;
}

bool CheckRight(int row, int col, double targetSquare)
{
    /*
    Console.WriteLine(">> CHECKS <<");
    Console.WriteLine($"Target Square: R{row}C{col} ({targetSquare})");
    */
    double obscuringTreeValue = 0;
    double previousTreeValue = -1;
    for (int i = colSize - 1; i > col; i--)
    {
        obscuringTreeValue = forestList[row][i];
        if (targetSquare <= obscuringTreeValue || obscuringTreeValue < previousTreeValue)
        {
            return false;
        }
        previousTreeValue = obscuringTreeValue;
    }
    return true;
}
bool CheckBottom(int row, int col, double targetSquare)
{
    /*
    Console.WriteLine(">> CHECKS <<");
    Console.WriteLine($"Target Square: R{row}C{col} ({targetSquare})");
    */
    double obscuringTreeValue = 0;
    double previousTreeValue = -1;
    for (int i = rowSize-1; i > row; i--)
    {
        obscuringTreeValue = forestList[i][col];
        if (targetSquare <= obscuringTreeValue || obscuringTreeValue < previousTreeValue)
        {
            return false;
        }
        previousTreeValue = obscuringTreeValue;
    }
    return true;
}


bool CheckLeft(int row, int col, double targetSquare)
{
    /*
    Console.WriteLine(">> CHECKS <<");
    Console.WriteLine($"Target Square: R{row}C{col} ({targetSquare})");
    */
    double obscuringTreeValue = 0;
    double previousTreeValue = -1;
    for (int i = 0; i < col; i++)
    {
        obscuringTreeValue = forestList[row][i];
        if (targetSquare <= obscuringTreeValue || obscuringTreeValue < previousTreeValue)
        {
            return false;
        }
        previousTreeValue = obscuringTreeValue;
    }
    return true;
}