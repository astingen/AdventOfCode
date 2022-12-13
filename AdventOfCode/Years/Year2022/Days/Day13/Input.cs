namespace AdventOfCode.Years.Year2022.Days.Day13;

public class Input
{
    public static IEnumerable<string> InputString
    {
        //get { return GetTestStrings(); }
        //get { return GetTestStringLineByLine(); }
        get { return System.IO.File.ReadLines(@"C:\Users\astin\Downloads\AoC\12-13-input.txt"); }
    }

    private static IEnumerable<string> GetTestStrings()
    {
        yield return @"";
    }

    private static IEnumerable<string> GetTestStringLineByLine()
    {
        return TEST_INPUT.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
    }

    private const string TEST_INPUT = @"[1,1,3,1,1]
[1,1,5,1,1]

[[1],[2,3,4]]
[[1],4]

[9]
[[8,7,6]]

[[4,4],4,4]
[[4,4],4,4,4]

[7,7,7,7]
[7,7,7]

[]
[3]

[[[]]]
[[]]

[1,[2,[3,[4,[5,6,7]]]],8,9]
[1,[2,[3,[4,[5,6,0]]]],8,9]";
}