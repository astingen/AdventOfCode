using JetBrains.Annotations;

namespace AdventOfCode.Years.Year2021.Days.Day18.Snailfish;

public sealed class SnailfishNumber : AbstractSnailfishComponent
{

    private const int EXPLODE_DEPTH = 4;

    [NotNull]
    public ISnailfishComponent LeftElement { get; set; }
    
    [NotNull]
    public ISnailfishComponent RightElement { get; set; }

    public SnailfishNumber(ISnailfishComponent leftElement, ISnailfishComponent rightElement) : 
        this(leftElement, rightElement, null, false)
    {
        
    }

    public SnailfishNumber(int leftNumber, ISnailfishComponent rightElement) : this(new RegularNumber(leftNumber),
        rightElement)
    {
        
    }

    public SnailfishNumber(ISnailfishComponent leftElement, int rightNumber) : this(leftElement,
        new RegularNumber(rightNumber))
    {
        
    }

    public SnailfishNumber(int leftNumber, int rightNumber) : this(new RegularNumber(leftNumber),
        new RegularNumber(rightNumber))
    {
        
    }


    public SnailfishNumber(ISnailfishComponent leftElement, ISnailfishComponent rightElement, SnailfishNumber? parent, bool isLeftSideOfParent)
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

        RegularNumber? numberToLeft = GetNumberToTheLeft();
        RegularNumber? numberToRight = GetNumberToTheRight();

        if (numberToLeft != null) 
            numberToLeft.Number += leftNumber.Number;
        if (numberToRight != null)
            numberToRight.Number += rightNumber.Number;
        
        ReplaceSelfWith(new RegularNumber(0));

        return true;
    }

    public override RegularNumber GetChildLeftMostNumber()
    {
        return LeftElement.GetChildLeftMostNumber();
    }

    public override RegularNumber GetChildRightMostNumber()
    {
        return RightElement.GetChildRightMostNumber();
    }

    public RegularNumber? GetNumberToTheLeft()
    {
        return GetLeftRoot()?.GetChildRightMostNumber();
    }

    public RegularNumber? GetNumberToTheRight()
    {
        return GetRightRoot()?.GetChildLeftMostNumber();
    }

    public ISnailfishComponent? GetLeftRoot()
    {
        if (Parent == null)
            return null;
        
        if (!IsLeftSideOfParent)
            return Parent.LeftElement;
        return Parent.GetLeftRoot();
    }

    public ISnailfishComponent? GetRightRoot()
    {
        if (Parent == null)
            return null;

        if (IsLeftSideOfParent)
            return Parent.RightElement;
        return Parent.GetRightRoot();
    }

    protected override string GetStringRepresentation()
    {
        return $"[{LeftElement.ToString()},{RightElement.ToString()}]";
    }

    public static SnailfishNumber FromString(string input)
    {
        string remainder;
        return SnailfishNumber.FromString(input, out remainder);
    }

    public static SnailfishNumber FromString(string input, out string remainder)
    {
        if (input[0] != '[')
            throw new FormatException("Snailfish numbers must start with a ']'");

        // Remove the opening '[' from working string;
        remainder = input.Substring(1);


        ISnailfishComponent leftElement = SnailfishNumber.ComponentFromString(remainder, out remainder, ',');
        ISnailfishComponent rightElement = SnailfishNumber.ComponentFromString(remainder, out remainder, ']');

        return new SnailfishNumber(leftElement, rightElement);
    }

    public static ISnailfishComponent ComponentFromString(string input, out string remainder, char endCharacter)
    {
        // Component is a regular number
        if (input[0] != '[')
        {
            int endIndex = input.IndexOf(endCharacter, 0);
            if (endIndex == -1)
                new FormatException("Couldn't find end of snailfish component");
            remainder = input.Substring(endIndex + 1);
            string componentNumber = input.Substring(0, endIndex);
            return new RegularNumber(int.Parse(componentNumber));
        }
        
        // Component is a snailfish number
        var number = FromString(input, out remainder);
        // Remove the end character
        if (remainder[0] != endCharacter)
            throw new FormatException();
        remainder = remainder.Substring(1);
        return number;
    }
}