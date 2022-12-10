﻿namespace AdventOfCode.Years.Year2021.Days.Day18.Starfish;

public abstract class AbstractStarfishComponent : IStarfishComponent
{
    public StarfishNumber? Parent { get; set; }
    public bool IsLeftSideOfParent { get; set; }
    public abstract bool IsRegularNumber { get; }
    public abstract int GetMagnitude();


    /// <summary>
    /// Split the number if needed
    /// Traverses the tree, left side first, until a split is needed. Returns true if a split is performed, false if not
    /// </summary>
    /// <returns>true if a split was performed, false if not</returns>
    public abstract bool Split();

    /// <summary>
    /// Explodes the number, if necessary.
    /// Recurses down the tree, left side first, until a number to explode is found
    /// Regular Numbers can't explode, so they always return false;
    /// </summary>
    /// <returns>true if an explode is performed, false if not</returns>
    public abstract bool Explode();

    public abstract RegularNumber GetChildLeftMostNumber();
    public abstract RegularNumber GetChildRightMostNumber();

    public void ReplaceSelfWith(IStarfishComponent replacement)
    {
        if (Parent == null)
            throw new InvalidOperationException("Can't Replace Root Number");

        replacement.Parent = Parent;
        replacement.IsLeftSideOfParent = IsLeftSideOfParent;

        if (IsLeftSideOfParent)
            Parent.LeftElement = replacement;
        else
            Parent.RightElement = replacement;
    }
}