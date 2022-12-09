namespace AdventOfCode.Years.Year2022.Days.Day08;

public static class Input
{
    public static IEnumerable<string> InputString
    {
        //get { return GetTestStrings(); }
        //get { return GetTestStringLineByLine(); }
        get { return System.IO.File.ReadLines(@"C:\Users\astin\Downloads\AoC\12-08-input.txt"); }
    }

    private static IEnumerable<string> GetTestStrings()
    {
        yield return @"";
    }

    private static IEnumerable<string> GetTestStringLineByLine()
    {
        return TEST_INPUT.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
    }

    private const string TEST_INPUT = @"30373
25512
65332
33549
35390";
}