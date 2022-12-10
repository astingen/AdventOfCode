using JetBrains.Annotations;

namespace AdventOfCode.Years.Year2021.Days.Day18.Starfish;

public interface IStarfishComponent
{
    StarfishNumber? Parent { get; set; }
    
    bool IsLeftSideOfParent { get; set; }
    
    bool IsRegularNumber { get; }

    int GetMagnitude();

    /// <summary>
    /// Split the number if needed
    /// Traverses the tree, left side first, until a split is needed. Returns true if a split is performed, false if not
    /// </summary>
    /// <returns>true if a split was performed, false if not</returns>
    bool Split();

    /// <summary>
    /// Explodes the number, if necessary.
    /// Recurses down the tree, left side first, until a number to explode is found
    /// Regular Numbers can't explode, so they always return false;
    /// </summary>
    /// <returns>true if an explode is performed, false if not</returns>
    bool Explode();

    RegularNumber GetChildLeftMostNumber();

    RegularNumber GetChildRightMostNumber();
}

public static class StarfishUtils
{
    public static StarfishNumber Add(StarfishNumber x, StarfishNumber y)
    { 
        var result = new StarfishNumber(x, y, null, false);
        x.Parent = result;
        x.IsLeftSideOfParent = true;
        y.Parent = result;
        y.IsLeftSideOfParent = false;
        return result;
    }
}