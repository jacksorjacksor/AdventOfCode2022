using System.Collections;

var path = "C:\\Users\\Richard\\OneDrive\\AZ-204\\PhotoProject\\AdventOfCode01\\AdventOfCode2022-02\\AdventOfCode2022-02\\AdventOfCode2022-02\\AoC2022Input02.txt";

var lines = System.IO.File.ReadAllLines(path);
var result = 0;

/*
 *  A X = Rock
 *  B Y = Paper
 *  C Z = Scissors
 *
 */

var dictionaryOfChoicePoints = new Dictionary<string, int>
{
    { "X", 1 },
    { "Y", 2 },
    { "Z", 3 }
};


foreach (var line in lines)
{
    var ourChoices = line.Split(' ');
    result += FightBitches(ourChoices[0], ourChoices[1]) + dictionaryOfChoicePoints[ourChoices[1]];
}

int FightBitches(string opponent, string hero)
{
    switch (opponent)
    {
        case "A":
            switch (hero)
            {
                case "X":
                    return 3;
                case "Y":
                    return 6;
                default:
                    return 0;
            };
        case "B":
            switch (hero)
            {
                case "X":
                    return 0;
                case "Y":
                    return 3;
                default:
                    return 6;
            };
        default:
            switch (hero)
            {
                case "X":
                    return 6;
                case "Y":
                    return 0;
                default:
                    return 3;
            };
    }
}

Console.WriteLine(result);