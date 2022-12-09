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


var path = Path.Combine(Directory.GetCurrentDirectory(), "AoC22-Day07-Input.csv");
var file = File.ReadAllLines(path);

var currentLevel = 0;
var highestLevel = 0;
var currentDir = "";

var listOfDir = new List<Dir>();

foreach (var line in file)
{
    Console.WriteLine(line);
    if (line.StartsWith("$"))
    {
        var body = line[2..].Trim();
        if (body.StartsWith("cd"))
        {
            switch (body)
            {
                case "cd /":
                    currentLevel = 0;
                    currentDir = body[2..].Trim(); // Assuming you can only find what is there
                    if(listOfDir.All(c => c.Name != currentDir)) listOfDir.Add(new Dir
                    {
                        Name = currentDir,
                        Children = new List<string>(),
                        Level = currentLevel,
                        Size = 0
                    });

                    break;
                case "cd ..":
                    currentLevel--;
                    break;
                default:
                    currentLevel++;
                    highestLevel = UpdateHighestLevel(currentLevel);
                    currentDir = body[2..].Trim(); // Assuming you can only find what is there
                    if (listOfDir.All(c => c.Name != currentDir)) listOfDir.Add(new Dir
                    {
                        Name = currentDir,
                        Children = new List<string>(),
                        Level = currentLevel,
                        Size = 0
                    });
                    break;
            }
        }
    }
    else
    {
        var lineSplit = line.Split(' ');
        var parentDir = listOfDir.Find(x => x.Name == currentDir);
        switch (lineSplit[0])
        {
            case "dir":
                parentDir?.Children.Add(lineSplit[1]);
                break;
            default:
                if (parentDir != null) parentDir.Size += int.Parse(lineSplit[0]);
                break;
        }
    }
}

int UpdateHighestLevel(int currentLevel)
{
    if(currentLevel > highestLevel)
    {
        return currentLevel;
    }
    else
    {
        return highestLevel;
    }
}

// Go back through the highest levels, add the values of their children to their Sizes

for (int i = highestLevel; i >= 0; i--)
{
    foreach (var dir in listOfDir.Where(x=> x.Level.Equals(i)))
    {
        // Go through children
        foreach (var childName in dir.Children)
        {
            foreach (var child in listOfDir.Where(child => child.Name.Equals(childName)))
            {
                dir.Size += child.Size;
            }
        }
    }
}

foreach (var item in listOfDir)
{
    Console.WriteLine(item);
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

// Too low: 1041286 // 1041286


internal class Dir
{
    public string Name { set; get; }
    public List<string> Children { set; get; }

    public int Size { set; get; }

    public int Level { set; get; }

    public override string ToString()
    {
        var childrenOutput = string.Join(",", Children);

        return $"Name: {Name} | Children: {childrenOutput} | Size: {Size} | Level: {Level}";
    }
}