using BigMath;
using BigInteger = System.Numerics.BigInteger;

namespace AdventOfCode.Years.Year2022.Days.Day11;

public class Item
{
    public Int256 Worry { get; set; }
    
    public long ThrowCount { get; set; }

    public Item(Int256 worry)
    {
        Worry = worry;
        ThrowCount = 0;
    }
}