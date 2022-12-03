using System.Collections;

var path = "C:\\Users\\Richard\\OneDrive\\AZ-204\\PhotoProject\\AdventOfCode01\\AdventOfCode2022-02\\AdventOfCode2022-02\\AdventOfCode2022-02\\AoC2022Input02.txt";

var lines = System.IO.File.ReadAllLines(path);
var result = 0;

/*
 *  A = Rock
 *  B = Paper
 *  C = Scissors
 *
 *  X = Lose
 *  Y = Draw
 *  Z = Win
 */

var dictionaryOfOutcomePoints = new Dictionary<string, int>
{
    { "X", 0 }, // Lose
    { "Y", 3 }, // Draw
    { "Z", 6 }  // Win
};

var dictionaryOfChoicePoints = new Dictionary<string, int>
{
    { "Rock", 1 },
    { "Paper", 2 },
    { "Scissors", 3 }
};



foreach (var line in lines)
{
    var ourInstructions = line.Split(' ');
    // Points for our outcome
    result += dictionaryOfOutcomePoints[ourInstructions[1]];

    // Work out what our choice needs to be
    Console.WriteLine(dictionaryOfChoicePoints[FightBitches(ourInstructions[0], ourInstructions[1])]);
    result += dictionaryOfChoicePoints[FightBitches(ourInstructions[0], ourInstructions[1])];
}

static string FightBitches(string opponent, string outcome)
{
    switch (opponent)           // WeHaveTo ('L', 'D', 'W')
    {
        case "A":               // Rock ("Scissors", "Rock", "Paper")
            return outcome switch
            {
                "X" => // Lose
                    "Scissors",
                "Y" => // Draw
                    "Rock",
                _ => "Paper"
            };

            ;
        case "B":               // Paper ("Rock", "Paper", "Scissors")
            return outcome switch
            {
                "X" => // Lose
                    "Rock",
                "Y" => // Draw
                    "Paper",
                _ => "Scissors"
            };

            ;
        default:                // Scissors ("Paper", "Scissors", "Rock")
            return outcome switch
            {
                "X" => // Lose
                    "Paper",
                "Y" => // Draw
                    "Scissors",
                _ => "Rock"
            };

            ;
    }
}

Console.WriteLine(result);