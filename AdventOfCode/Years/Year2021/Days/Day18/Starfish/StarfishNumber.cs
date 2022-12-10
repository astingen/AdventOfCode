using System.ComponentModel;
using JetBrains.Annotations;

namespace AdventOfCode.Years.Year2021.Days.Day18.Starfish;

public sealed class StarfishNumber : AbstractStarfishComponent
{

    private const int EXPLODE_DEPTH = 4;

    [NotNull]
    public IStarfishComponent LeftElement { get; set; }
    
    [NotNull]
    public IStarfishComponent RightElement { get; set; }

    public StarfishNumber(IStarfishComponent leftElement, IStarfishComponent rightElement) : 
        this(leftElement, rightElement, null, false)
    {
        
    }


    public StarfishNumber(IStarfishComponent leftElement, IStarfishComponent rightElement, StarfishNumber? parent, bool isLeftSideOfParent)
    {
        if (leftElement == null) throw new ArgumentNullException(nameof(leftElement));
        if (rightElement == null) throw new ArgumentNullException(nameof(rightElement));
        Parent = parent;
        IsLeftSideOfParent = isLeftSideOfParent;
        LeftElement = leftElement;
        RightElement = rightElement;
        LeftElement.Parent = this;
        LeftElement.IsLeftSideOfParent = true;
        RightElement.Parent = this;
        RightElement.IsLeftSideOfParent = false;
    }

    public override bool IsRegularNumber
    {
        get { return false; }
    }

    public override int GetMagnitude()
    {
        return (LeftElement.GetMagnitude() * 3) + (RightElement.GetMagnitude() * 2);
    }

    /// <summary>
    /// Split the number if needed
    /// Traverses the tree, left side first, until a split is needed. Returns true if a split is performed, false if not
    /// </summary>
    /// <returns>true if a split was performed, false if not</returns>
    public override bool Split()
    {
        return LeftElement.Split() || RightElement.Split();
    }

    /// <summary>
    /// Gets the depth of this component
    /// </summary>
    /// <returns></returns>
    public int GetDepth()
    {
        if (Parent == null)
            return 0;
        return Parent.GetDepth() + 1;
    }

    /// <summary>
    /// Explodes the number, if necessary.
    /// Recurses down the tree, left side first, until a number to explode is found
    /// </summary>
    /// <returns>true if an explode is performed, false if not</returns>
    public override bool Explode()
    {
        // Check if this is a pair of regular numbers
        var leftNumber = LeftElement as RegularNumber;
        var rightNumber = RightElement as RegularNumber;

        // This is not a pair of regular numbers, continue to traverse the tree
        if (leftNumber == null || rightNumber == null)
        {
            return LeftElement.Explode() || RightElement.Explode();
        }
        
        // If we're not at the explode depth, return false
        if (GetDepth() < EXPLODE_DEPTH)
            return false;
        
        // Explode the number
        //var numberToTheLeft = 

        throw new NotImplementedException();
    }

    public override RegularNumber GetChildLeftMostNumber()
    {
        return LeftElement.GetChildLeftMostNumber();
    }

    public override RegularNumber GetChildRightMostNumber()
    {
        return RightElement.GetChildRightMostNumber();
    }

    public RegularNumber? GetParentsLeftMostNumber()
    {
        if (Parent == null)
            return null;

        if (IsLeftSideOfParent)
            return Parent.GetParentsLeftMostNumber();
        return Parent.GetChildLeftMostNumber();
    }

    public RegularNumber? GetParentsRightMostNumber()
    {
        if (Parent == null)
            return null;

        if (IsLeftSideOfParent)
            return Parent.GetChildRightMostNumber();
        return Parent.GetParentsRightMostNumber();

    }
}