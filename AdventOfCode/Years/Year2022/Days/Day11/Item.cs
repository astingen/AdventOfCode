using BigInteger = System.Numerics.BigInteger;

namespace AdventOfCode.Years.Year2022.Days.Day11;

public class Item
{
    public long Worry { get; set; }
    
    public long ThrowCount { get; set; }

    public Item(long worry)
    {
        Worry = worry;
        ThrowCount = 0;
    }
}