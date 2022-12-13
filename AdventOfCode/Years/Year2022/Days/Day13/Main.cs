using System.Text.Json;
using AdventOfCode.Extensions;

namespace AdventOfCode.Years.Year2022.Days.Day13;

public class Main
{
    public static void Run()
    {
        var inputs = Input.InputString.GetEnumerator();

        int count = 0;
        int index = 1;

        while (true)
        {
            if (!inputs.MoveNext())
                break;
            var line1 = inputs.Current;
            if (string.IsNullOrWhiteSpace(line1))
                continue;

            if (!inputs.MoveNext())
                throw new FormatException();

            var line2 = inputs.Current;

            var line1Json = JsonDocument.Parse(line1).RootElement;
            var line2Json = JsonDocument.Parse(line2).RootElement;

            var result = CompareItems(line1Json, line2Json);
            
            if (!result.HasValue)
                Console.WriteLine("Return No Value");
            if (result.HasValue && result.Value) 
                count += index;

            index++;
        }
        
        inputs.Dispose();
        
        Console.WriteLine($"Counts Sum:{count}");
    }

    public static void RunPart2()
    {
        List<JsonElement> elements = new List<JsonElement>();

        foreach (var line in Input.InputString)
        {
            if (!string.IsNullOrWhiteSpace(line))
                elements.Add(JsonDocument.Parse(line).RootElement);
        }

        var divider1 = JsonDocument.Parse("[[2]]").RootElement;
        var divider2 = JsonDocument.Parse("[[6]]").RootElement;
        
        elements.Add(divider1);
        elements.Add(divider2);

        var sortedElements = elements.OrderBy(k => k, PacketComparer.Instance).ToList();

        var divider1Index = sortedElements.IndexOf(divider1) + 1;
        var divider2Index = sortedElements.IndexOf(divider2) + 1;
        var output = divider1Index * divider2Index;

        Console.WriteLine($"Output: {output}");
    }

    /// <summary>
    /// Compare 2 iems, null means continue comparing
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool? CompareItems(JsonElement left, JsonElement right)
    {
        // Check Raw Values
        if (left.ValueKind == JsonValueKind.Number && right.ValueKind == JsonValueKind.Number)
        {
            int leftNumber = left.GetInt32();
            int rightNumber = right.GetInt32();
            if (leftNumber < rightNumber)
                return true;
            if (leftNumber > rightNumber)
                return false;
            
            return null;
        }

        
        // Check For Nulls
        if (left.ValueKind == JsonValueKind.Null && right.ValueKind == JsonValueKind.Null)
            return null;

        if (left.ValueKind == JsonValueKind.Null || right.ValueKind == JsonValueKind.Null)
            return left.ValueKind == JsonValueKind.Null;
        
        
        
        // Check for only one number
        if (left.ValueKind == JsonValueKind.Number)
        {
            int leftNumber = left.GetInt32();
            left = ConvertToArray(leftNumber);
        }

        if (right.ValueKind == JsonValueKind.Number)
        {
            int rightNumber = right.GetInt32();
            right = ConvertToArray(rightNumber);
        }
        
        // Check Elements
        var leftArray = left.EnumerateArray().ToArray();
        var leftArrayLength = left.GetArrayLength();
        var rightArray = right.EnumerateArray().ToArray();
        var rightArrayLength = right.GetArrayLength();

        int iterations = Math.Min(leftArrayLength, rightArrayLength);

        for (int i = 0; i < iterations; i++)
        {
            var compareResult = CompareItems(left[i], right[i]);
            if (compareResult.HasValue)
                return compareResult;
        }

        if (leftArrayLength == rightArrayLength)
            return null;
        
        return leftArrayLength < rightArrayLength;
        
    }

    public static JsonElement ConvertToArray(int input)
    {
        return JsonDocument.Parse($"[{input}]").RootElement;
    }
}

public class PacketComparer : IComparer<JsonElement>
{
    private static readonly PacketComparer s_Instance;

    static PacketComparer()
    {
        s_Instance = new PacketComparer();
    }
    
    public static PacketComparer Instance
    {
        get { return s_Instance; }
    }
    
    /// <summary>Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.</summary>
    /// <param name="x">The first object to compare.</param>
    /// <param name="y">The second object to compare.</param>
    /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.
    /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description><paramref name="x" /> is less than <paramref name="y" />.</description></item><item><term> Zero</term><description><paramref name="x" /> equals <paramref name="y" />.</description></item><item><term> Greater than zero</term><description><paramref name="x" /> is greater than <paramref name="y" />.</description></item></list></returns>
    public int Compare(JsonElement x, JsonElement y)
    {
        var result = Main.CompareItems(x, y);
        if (!result.HasValue)
            return 0;
        return result.Value ? -1 : 1;
    }
}