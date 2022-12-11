var path = Path.Combine(Directory.GetCurrentDirectory(), "AoC22Day11Input.csv");
var file = File.ReadAllLines(path);

var listOfMonkeys = new List<Monkey>();
MonkeyMaker(file);

// Monkey check
Console.WriteLine("Starting monkeys!");
foreach (var monkey in listOfMonkeys) Console.WriteLine(monkey);

// START!
var numberOfRounds = 20;

for (int i = 0; i < numberOfRounds; i++)
{
    foreach (var monkey in listOfMonkeys)
    {
        // "If a monkey is holding no items at the start of its turn, its turn ends."
        while (!monkey.itemsHeld.Count.Equals(0))
        {
            Console.WriteLine("* * *");
            Console.WriteLine(monkey);

            // Gets first item in list (pop)
            var item = monkey.itemsHeld.Dequeue(); // opposite = Enqueue
            Console.WriteLine($"1. Get item");
            Console.WriteLine($"Worry level of item: {item.worryLevel}");

            Console.WriteLine("Monkey inspects item:");
            monkey.inspectionCount++;

            // Inspects item ("operation")
            item.worryLevel = Operation(monkey.Id, item.worryLevel);
            Console.WriteLine($"2. Operation");
            Console.WriteLine($"Worry level of item: {item.worryLevel}");

            // Destress as item not broken
            item.worryLevel = Relief(item.worryLevel);
            Console.WriteLine($"3. Relief");
            Console.WriteLine($"Worry level of item: {item.worryLevel}");

            // Test
            var testOutcome = item.worryLevel % monkey.testDivisor == 0;
            Console.WriteLine("4. Test");
            Console.WriteLine($"Outcome: {testOutcome}");

            // Outcome
            var monkeyToThrowTo = testOutcome ? MonkeyFinder(monkey.trueMonkeyId) : MonkeyFinder(monkey.falseMonkeyId);
            Console.WriteLine($"5. New monkey thrown to:");
            Console.WriteLine(monkeyToThrowTo);

            // Enqueue item
            monkeyToThrowTo.itemsHeld.Enqueue(item);
            Console.WriteLine("6. New monkey holds item");
            Console.WriteLine(monkeyToThrowTo);

            // Total stress of new monkey
            Console.WriteLine("Total stress level:");
            Console.WriteLine(monkey.TotalStressLevel());
        }
    }
}

// Monkey check
Console.WriteLine("Ending monkeys!");
foreach (var monkey in listOfMonkeys) Console.WriteLine(monkey);

// Monkey business
var monkeyBusinessList = listOfMonkeys.OrderByDescending(m => m.inspectionCount).Take(2).ToList();
var monkeyBusiness = monkeyBusinessList[0].inspectionCount * monkeyBusinessList[1].inspectionCount;

Console.WriteLine($"Final value - Monkey Business!!: {monkeyBusiness}");
// This I am hard coding as I don't understand enough about updating C# class methods. Yes I am a terrible person.
int Operation(int id, int worry)
{
    return id switch
    {
        0 => worry * 11,
        1 => worry + 1,
        2 => worry * 7,
        3 => worry + 3,
        4 => worry + 6,
        5 => worry + 5,
        6 => worry * worry,
        7 => worry + 7,
        _ => -1
    };
}

int Relief(int worry)
{
    return (int)worry/3;
}

// Populates the list of monkeys
void MonkeyMaker(string[] file)
{
    var monkeyCounter = 0;
    // Assemble the monkeys and create the items
    foreach (var line in file)
    {
        var splitLine = line.Trim().Split();
        if (line.Equals("")) monkeyCounter++;

        else if (splitLine[0].Equals("Monkey"))
        {
            listOfMonkeys.Add(new Monkey()
            {
                Id = int.Parse(splitLine[1][..1]),
                itemsHeld = new Queue<Item>(),
                testDivisor = null,
                trueMonkeyId = null,
                falseMonkeyId = null,
                inspectionCount = 0,
            });
        }

        else if (splitLine[0].Equals("Starting"))
        {
            var currentMonkey = MonkeyFinder(monkeyCounter);
            for (int i = 2; i < splitLine.Length; i++)
            {
                int value = int.Parse(splitLine[i].Replace(",", ""));
                var itemToAdd = new Item() { worryLevel = value };
                currentMonkey.itemsHeld.Enqueue(itemToAdd);
            }
        }

        else if (splitLine[0].Equals("Test:"))
        {
            var currentMonkey = MonkeyFinder(monkeyCounter);
            currentMonkey.testDivisor = int.Parse(splitLine[3]);
        }

        else if (splitLine[1].Equals("true:"))
        {
            MonkeyFinder(monkeyCounter).trueMonkeyId = int.Parse(splitLine[5]);
        }

        else if (splitLine[1].Equals("false:"))
        {
            MonkeyFinder(monkeyCounter).falseMonkeyId = int.Parse(splitLine[5]);
        }
    }
}


Monkey MonkeyFinder(int? id)
{
    return (listOfMonkeys?.Find(x => x.Id == id) ?? null)!;
}

class Monkey
{
    public int Id { get; set; }
    public Queue<Item> itemsHeld { get; set; }

    public int? testDivisor {get; set; }

    public int? trueMonkeyId { get; set; }
    public int? falseMonkeyId { get; set; }

    public int inspectionCount { get; set; }

    public int TotalStressLevel()
    {
        return itemsHeld.Sum(item => item.worryLevel);
    }

    public override string ToString()
    {
        return
            $"Id:\t{Id}\t" +
            $"Items held:\t{itemsHeld.Count}" +
            $"\tTest div:\t{testDivisor}\t" +
            $"\tInspection count:\t{inspectionCount}\t" +
            $"True Monkey:\t{trueMonkeyId}\t" +
            $"False Monkey:\t{falseMonkeyId}";
    }
}

class Item
{
    public int worryLevel { get; set; }
}



