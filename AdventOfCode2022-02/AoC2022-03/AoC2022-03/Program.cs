var path =
    "C:\\Users\\Richard\\OneDrive\\AZ-204\\PhotoProject\\AdventOfCode01\\AdventOfCode2022-02\\AoC2022-03\\AoC2022-03\\AoC2022-03-input.txt";
var file = System.IO.File.ReadAllLines(path);

/*
Each rucksack has two large compartments.
All items of a given type are meant to go into exactly one of the two compartments.
The Elf that did the packing failed to follow this rule for exactly one item type per rucksack.

The Elves have made a list of all of the items currently in each rucksack (your puzzle input), but they need your help finding the errors.
Every item type is identified by a single lowercase or uppercase letter (that is, a and A refer to different types of items).

The list of items for each rucksack is given as characters all on a single line.
A given rucksack always has the same number of items in each of its two compartments, so the first half of the characters represent items in the first compartment, while the second half of the characters represent items in the second compartment.

For example, suppose you have the following list of contents from six rucksacks:

vJrwpWtwJgWrhcsFMMfFFhFp
jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
PmmdzqPrVvPwwTWBwg
wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
ttgJtRGJQctTZtZT
CrZsJsPPZsGzwwsLwLmpwMDw

    The first rucksack contains the items vJrwpWtwJgWrhcsFMMfFFhFp, which means its first compartment contains the items vJrwpWtwJgWr, while the second compartment contains the items hcsFMMfFFhFp. The only item type that appears in both compartments is lowercase p.
    The second rucksack's compartments contain jqHRNqRjqzjGDLGL and rsFMfFZSrLrFZsSL. The only item type that appears in both compartments is uppercase L.
    The third rucksack's compartments contain PmmdzqPrV and vPwwTWBwg; the only common item type is uppercase P.
    The fourth rucksack's compartments only share item type v.
    The fifth rucksack's compartments only share item type t.
    The sixth rucksack's compartments only share item type s.

To help prioritize item rearrangement, every item type can be converted to a priority:

    Lowercase item types a through z have priorities 1 through 26.
    Uppercase item types A through Z have priorities 27 through 52.

In the above example, the priority of the item type that appears in both compartments of each rucksack is 16 (p), 38(L), 42(P), 22(v), 20(t), and 19(s); the sum of these is 157.

Find the item type that appears in both compartments of each rucksack.
What is the sum of the priorities of those item types?

 */

var result = 0;

var bag1 = "";
var bag2 = "";
var bag3 = "";

foreach (var line in file)
{
    if (bag1 == "")
    {
        bag1 = line;
        Console.WriteLine("Bag1: " + bag1);
    }

    else if (bag2 == "")
    {
        bag2 = line;
        Console.WriteLine("Bag2: " + bag2);
    }

    else
    {
        bag3 = line;
        Console.WriteLine("Bag3: " + bag3);
        // All three bags must now be assigned
        // Find character
        foreach (var item in bag1)
        {
            if (bag2.Contains(item) && bag3.Contains(item))
            {
                result += AssignPriority(item);
                break;
            }
        }

        Console.WriteLine("Reset!");

        // Reset
        bag1 = "";
        bag2 = "";
        bag3 = "";
    }
}

Console.WriteLine(result);

static int AssignPriority(char a)
{
    var outputNumber = (int)a - 64;
    if (outputNumber < 27)
    {
        outputNumber += 26;
    }
    else
    {
        outputNumber -= 32;
    }

    Console.WriteLine("Item: " + a + " | Priority: " + outputNumber);
    return outputNumber;
}