using System.ComponentModel;
using System.Data.SqlTypes;

namespace AdventOfCode.Years.Year2021.Days.Day18.Starfish;

public class RegularNumber : AbstractStarfishComponent
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
    /// Gets the starfish component for the given regular number
    /// Splits the component and returns component pieces if necessary
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static IStarfishComponent GetCompnent(int number)
    {
        if (number < 0)
            throw new ArgumentOutOfRangeException(nameof(number));

        if (number < 10)
            return new RegularNumber(number);

        // Split the number
        
        int roundDown = number / SPLIT_DIVISOR;
        int remainder = number % SPLIT_DIVISOR;

        int roundUp = roundDown;
        if (remainder >= SPLIT_DIVISOR / 2)
            roundUp++;

        return new StarfishNumber(new RegularNumber(roundDown), new RegularNumber(roundUp));
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
}