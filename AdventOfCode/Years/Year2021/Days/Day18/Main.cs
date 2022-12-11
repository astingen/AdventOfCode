using AdventOfCode.Years.Year2021.Days.Day18.Snailfish;

namespace AdventOfCode.Years.Year2021.Days.Day18;

public class Main
{
    public void Run()
    {
        SnailfishNumber[] snailfishNumbers = Input.InputString.Select(SnailfishNumber.FromString).ToArray();
        //var sum = snailfishNumbers.Aggregate((result, item) => SnailfishUtils.Add(result, item));
        
        //Console.WriteLine($"Sum: {sum}");
        //Console.WriteLine($"Magnitude: {sum.GetMagnitude()} (pop pop!)");

        int largestMagnitude = 0;

        foreach (var number in snailfishNumbers)
        {
            foreach (var sumNumber in snailfishNumbers.Where(a => a != number))
            {
                var thisSum = SnailfishUtils.Add(number, sumNumber);
                int thisMagnitude = thisSum.GetMagnitude();
                if (thisMagnitude > largestMagnitude)
                    largestMagnitude = thisMagnitude;
            }
        }
        
        Console.WriteLine($"Largest Magnitude: {largestMagnitude}");

    }
}