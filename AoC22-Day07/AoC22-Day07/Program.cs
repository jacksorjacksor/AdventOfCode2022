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
var currentDir = "";

var arrayOfDictionaries = new Dictionary<string, Dir>();


foreach (var line in file)
{
    if (line.StartsWith("$"))
    {
        var body = line.Substring(2);
        if (body.StartsWith("cd"))
        {
            switch (body)
            {
                case "cd /":
                    currentLevel = 0;
                    currentDir = body.Substring(2); // Assuming you can only find what is there

                    if (!arrayOfDictionaries.ContainsKey(currentDir)) arrayOfDictionaries.Add(currentDir, new Dir
                    {
                        name=currentDir, children=new ArrayList(), size=0
                    });
                    break;
                case "cd ..":
                    currentLevel--;
                    break;
                default:
                    currentLevel++;
                    currentDir = body.Substring(2); // Assuming you can only find what is there
                    if (!arrayOfDictionaries.ContainsKey(currentDir)) arrayOfDictionaries.Add(currentDir, new Dir
                    {
                        name = currentDir,
                        children = new ArrayList(),
                        size = 0
                    }); 
                    break;
            }
        }
    }
    else
    {
        // values being shown
        if (line.StartsWith("d"))
        {
        }
    }
}

foreach (var item in arrayOfDictionaries)
{
    Console.WriteLine(item.Value);
}

// List all the files and directories



// Find the level you're on
// Root = 0, "/" sets to 1, "[letter]" adds 1, ".." subtracts 1
// ls is ignored


internal class Dir
{
    public string name { set; get; }
    public ArrayList children { set; get; }

    public int size { set; get; }

    public override string ToString()
    {
        var childrenOutput = "";
        foreach (var child in children)
        {
            childrenOutput += child + ",";
        }
        return $"Name: {name} | Children: {childrenOutput} | Size: {size}";
    }
}