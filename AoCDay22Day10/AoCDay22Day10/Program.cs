var path = Path.Join(Directory.GetCurrentDirectory(),"Day22Input.csv");
var file = File.ReadAllLines(path);

// The clock circuit ticks at a constant rate; each tick is called a cycle

// Start by figuring out the signal being sent by the CPU

var register = 1;
var cycle = 0;
var signalStrength = 0;

var listOfCyclesToCheck = new List<int> { 20, 60, 100, 140, 180, 220 };

/*
    addx V takes two cycles to complete. After two cycles, the X register is increased by the value V. (V can be negative.)
    noop takes one cycle to complete. It has no other effect.
*/

foreach (var line in file)
{
    if (line.Equals("noop"))
    {
        CycleUpdate();
    }
    else // addx
    {
        var valueToIncreaseRegister = int.Parse(line.Split()[1]);

        for (int i = 0; i < 2; i++)
        {
            CycleUpdate();
        }

        // Update
        register += valueToIncreaseRegister;
    }
}

Console.WriteLine(signalStrength);

void CycleUpdate()
{
    cycle++;
    if (listOfCyclesToCheck.Contains(cycle))
    {
        signalStrength += register * cycle;
    }
}