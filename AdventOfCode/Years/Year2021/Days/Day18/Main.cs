using AdventOfCode.Years.Year2021.Days.Day18.Snailfish;

namespace AdventOfCode.Years.Year2021.Days.Day18;

public class Main
{
    public void Run()
    {
        var snailfishNumbers = Input.InputString.Select(s => SnailfishNumber.FromString(s));
        var sum = snailfishNumbers.Aggregate((result, item) => SnailfishUtils.Add(result, item));
        
        Console.WriteLine($"Sum: {sum}");
        Console.WriteLine($"Magnitude: {sum.GetMagnitude()} (pop pop!)");
        
    }
}