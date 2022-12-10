using JetBrains.Annotations;

namespace AdventOfCode.Years.Year2021.Days.Day18.Starfish;

public interface IStarfishComponent
{
    StarfishNumber? Parent { get; set; }
    
    bool IsLeftSideOfParent { get; set; }
    
    bool IsRegularNumber { get; }

    int GetMagnitude();
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