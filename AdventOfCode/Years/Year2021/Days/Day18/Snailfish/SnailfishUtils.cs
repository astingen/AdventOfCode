namespace AdventOfCode.Years.Year2021.Days.Day18.Snailfish;

public static class SnailfishUtils
{
    public static SnailfishNumber Add(SnailfishNumber x, SnailfishNumber y)
    {
        var result = new SnailfishNumber(x.Clone(), y.Clone());
        Reduce(result);
        return result;
    }

    public static void Reduce(SnailfishNumber number)
    {
        while (true)
        {
            if (number.Explode())
            {
                //Console.WriteLine($"after explode: {number}");
                continue;
            }

            if (number.Split())
            {
                //Console.WriteLine($"after split:   {number}");
                continue;
            }
            break;
        }
    }
}