namespace AdventOfCode.Years.Year2022.Days.Day09;

public static class Input
{
    public static IEnumerable<string> InputString
    {
        //get { return GetTestStrings(); }
        //get { return GetTestStringLineByLine(); }
        get { return System.IO.File.ReadLines(@"C:\Users\astin\Downloads\AoC\12-09-input.txt"); }
    }

    private static IEnumerable<string> GetTestStrings()
    {
        yield return @"";
    }

    private static IEnumerable<string> GetTestStringLineByLine()
    {
        return TEST_INPUT.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
    }

    private const string TEST_INPUT = @"R 5
U 8
L 8
D 3
R 17
D 10
L 25
U 20";
}