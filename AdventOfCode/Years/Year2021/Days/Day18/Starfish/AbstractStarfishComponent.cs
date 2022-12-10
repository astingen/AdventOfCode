namespace AdventOfCode.Years.Year2021.Days.Day18.Starfish;

public abstract class AbstractStarfishComponent : IStarfishComponent
{
    public StarfishNumber? Parent { get; set; }
    public bool IsLeftSideOfParent { get; set; }
    public abstract bool IsRegularNumber { get; }
    public abstract int GetMagnitude();

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