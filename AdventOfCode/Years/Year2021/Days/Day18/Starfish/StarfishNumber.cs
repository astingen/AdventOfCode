using System.ComponentModel;
using JetBrains.Annotations;

namespace AdventOfCode.Years.Year2021.Days.Day18.Starfish;

public class StarfishNumber : AbstractStarfishComponent
{

    [NotNull]
    public IStarfishComponent LeftElement { get; set; }
    
    [NotNull]
    public IStarfishComponent RightElement { get; set; }


    public StarfishNumber(IStarfishComponent leftElement, IStarfishComponent rightElement, StarfishNumber? parent, bool isLeftSideOfParent)
    {
        if (leftElement == null) throw new ArgumentNullException(nameof(leftElement));
        if (rightElement == null) throw new ArgumentNullException(nameof(rightElement));
        Parent = parent;
        IsLeftSideOfParent = isLeftSideOfParent;
        LeftElement = leftElement;
        RightElement = rightElement;
    }

    public override bool IsRegularNumber
    {
        get { return false; }
    }

    public override int GetMagnitude()
    {
        return (LeftElement.GetMagnitude() * 3) + (RightElement.GetMagnitude() * 2);
    }
}