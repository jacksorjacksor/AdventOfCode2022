var path = Path.Join(Directory.GetCurrentDirectory(),"Day22Input.csv");
var file = File.ReadAllLines(path);

// The clock circuit ticks at a constant rate; each tick is called a cycle

// Start by figuring out the signal being sent by the CPU

var register = 1;
var cycle = 0;
var spriteCentre = 0;
var listOfPixels = new List<Pixel>();
/*
    addx V takes two cycles to complete. After two cycles, the X register is increased by the value V. (V can be negative.)
    noop takes one cycle to complete. It has no other effect.
*/

foreach (var line in file)
{
    if (line.Equals("noop"))
    {
        CycleUpdate(line);
    }
    else // addx
    {
        var valueToIncreaseRegister = int.Parse(line.Split()[1]);

        for (int i = 0; i < 2; i++)
        {
            CycleUpdate(line);
        }

        // Update
        register += valueToIncreaseRegister;
    }
}

// Creates output string of "#" and "."
string output = listOfPixels.Aggregate("", (current, pixel) => current + pixel.symbol);
Console.WriteLine(output);
Console.WriteLine("****");
Console.WriteLine(output.Substring(0,40));
Console.WriteLine(output.Substring(40,40));
Console.WriteLine(output.Substring(80,40));
Console.WriteLine(output.Substring(120,40));
Console.WriteLine(output.Substring(160,40));
Console.WriteLine(output.Substring(200,40));


void CycleUpdate(string line)
{
    cycle++;
    var adjustedCycle = cycle % 40;
    var isPixelFound = adjustedCycle >= register && adjustedCycle <= register + 2;
    listOfPixels.Add(new Pixel()
    {
        cycle = cycle,
        lit = isPixelFound,
        symbol = isPixelFound ? "#" : "."
    });


    Console.WriteLine("*********");
    /*Console.WriteLine($"" +
                      $"Cycle: {cycle}\t" +
                      $"Adjusted: {adjustedCycle}\t" +
                      $"Register: {register}\t" +
                      $"Found: {isPixelFound}"
                      );*/

    Console.WriteLine($"Start cycle\t {cycle}: begin executing {line}");
    Console.WriteLine($"During cycle\t {cycle}: CRT draws pixel in position {register} (range: {register-1} - {register+1})");
    Console.WriteLine($"Current CRT row:\t{listOfPixels.Aggregate("", (current, pixel) => current + pixel.symbol)}");

    // Check here if the pixel is +/- 1 of register

}

class Pixel
{
    public int cycle { get; set; }
    public bool lit { get; set; }
    public string symbol { get; set; }
}