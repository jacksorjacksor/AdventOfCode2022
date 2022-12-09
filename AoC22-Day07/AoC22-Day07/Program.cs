/*
Test Input:
$ cd /
$ ls
dir a
14848514 b.txt
8504156 c.dat
dir d
$ cd a
$ ls
dir e
29116 f
2557 g
62596 h.lst
$ cd e
$ ls
584 i
$ cd ..
$ cd ..
$ cd d
$ ls
4060174 j
8033020 d.log
5626152 d.ext
7214296 k

*/
/*
Becomes:
- / (dir)
  - a (dir)
    - e (dir)
      - i (file, size=584)
    - f (file, size=29116)
    - g (file, size=2557)
    - h.lst (file, size=62596)
  - b.txt (file, size=14848514)
  - c.dat (file, size=8504156)
  - d (dir)
    - j (file, size=4060174)
    - d.log (file, size=8033020)
    - d.ext (file, size=5626152)
    - k (file, size=7214296)
*/

using System.Collections;
using System.Reflection;
using System.Threading.Channels;


var path = Path.Combine(Directory.GetCurrentDirectory(), "AoC22-Day07-SampleInput.csv");
var file = File.ReadAllLines(path);

var currentLevel = 0;
var highestLevel = 0;

var listOfDir = new List<Dir>();
Dir currentDir = null;
foreach (var line in file)
{
    /*
    Console.WriteLine(line);
    */
    if (line.StartsWith("$"))
    {
        var body = line[2..].Trim();
        if (body.StartsWith("cd"))
        {
            switch (body)
            {
                case "cd ..":
                    currentLevel--;
                    break;

                case "cd /":
                    currentLevel = 0;

                    currentDir = new Dir {
                        Name = body[2..].Trim(),
                        Parent = null,
                        Level = currentLevel,
                        Size = 0
                    };

                    listOfDir.Add(currentDir);

                    break;

                default:
                    currentLevel++;
                    highestLevel = UpdateHighestLevel(currentLevel);



                    currentDir = new Dir
                    {
                        Name = body[2..].Trim(),
                        Parent = currentDir,
                        Level = currentLevel,
                        Size = 0
                    };

                    listOfDir.Add(currentDir);

                    break;
            }
        }
    }
    else
    {

        var lineSplit = line.Split(' ');
        switch (lineSplit[0])
        {
            case "dir":
                // currentDir?.Children.Add(lineSplit[1]);
                break;
            default:
                if (currentDir != null) currentDir.Size += int.Parse(lineSplit[0]);
                break;
        }
    }
}

int UpdateHighestLevel(int level)
{
    if(level > highestLevel)
    {
        return level;
    }
    else
    {
        return highestLevel;
    }
}

// 
foreach (var dir in listOfDir)
{
    Console.WriteLine(dir);
}


Console.WriteLine("*** Adjusted ***");
// Go back through the highest levels, add the values of their children to their Sizes

for (int i = highestLevel; i >= 0; i--)
{
    foreach (var dir in listOfDir.Where(x=> x.Level.Equals(i)))
    {
        if (dir.Parent != null) dir.Parent.Size += dir.Size;
        Console.WriteLine(dir);
    }
}


Console.WriteLine("*****");

// Remove any where the Size>=100000
var grandTotal = 0;
foreach (var dir in listOfDir.Where(x => x.Size <= 100000))
{
    Console.WriteLine(dir);
    grandTotal += dir.Size;
}

Console.WriteLine(grandTotal);

// Too low: 1041286 // 1771828


internal class Dir
{
    public string Name { set; get; }
    public Dir? Parent { set; get; }
    public int Size { set; get; }

    public int Level { set; get; }

    public override string ToString()
    {
        return $"Name: {Name} | Parent: {Parent?.Name} | Size: {Size} | Level: {Level}";
    }
}