// 98-99,3-97 = not contained
// 1-5,2-4 = contained
// In how many assignment pairs does one range fully contain the other?

var path = Path.Combine(Directory.GetCurrentDirectory(),"AoC22-Day04.csv");

var file = File.ReadAllLines(path);

var outcome = 0;

foreach (var line in file)
{
    var part1 = line.Split(",")[0];
    var part2 = line.Split(",")[1];
    var part1a = int.Parse(part1.Split("-")[0]);
    var part1b = int.Parse(part1.Split("-")[1]);
    var part2a = int.Parse(part2.Split("-")[0]);
    var part2b = int.Parse(part2.Split("-")[1]);

    if ((part1a <= part2a && part1b >= part2b) || (part2a <= part1a && part2b >= part1b)) outcome++;
}

Console.WriteLine(outcome);