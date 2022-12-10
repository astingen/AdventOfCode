using System.ComponentModel;
using System.Data.SqlTypes;

namespace AdventOfCode.Years.Year2021.Days.Day18.Starfish;

public sealed class RegularNumber : AbstractStarfishComponent
{

    public const int SPLIT_DIVISOR = 2;
    public override bool IsRegularNumber {
        get { return true; }
    }

    public int Number { get; set; }

    public RegularNumber(int number)
    {
        Number = number;
    }

    /// <summary>
    /// Splits the number
    /// </summary>
    /// <returns></returns>
    public override bool Split()
    {
        if (Number < 10)
            return false;

        // Split the number
        
        int roundDown = Number / SPLIT_DIVISOR;
        int remainder = Number % SPLIT_DIVISOR;

        int roundUp = roundDown;
        if (remainder >= SPLIT_DIVISOR / 2)
            roundUp++;
        
        var result = new StarfishNumber(new RegularNumber(roundDown), new RegularNumber(roundUp));
        ReplaceSelfWith(result);
        return true;
    }

    public int GetLeftMostNumber()
    {
        return Number;
    }

    public int GetRightMostNumber()
    {
        return Number;
    }

    public override int GetMagnitude()
    {
        return Number;
    }

    /// <summary>
    /// Explodes the number, if necessary.
    /// Recurses down the tree, left side first, until a number to explode is found
    /// Regular Numbers can't explode, so they always return false;
    /// </summary>
    /// <returns>true if an explode is performed, false if not</returns>
    public override bool Explode()
    {
        return false;
    }

    public override RegularNumber GetChildLeftMostNumber()
    {
        return this;
    }

    public override RegularNumber GetChildRightMostNumber()
    {
        return this;
    }
}