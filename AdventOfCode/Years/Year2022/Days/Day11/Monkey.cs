using BigMath;

namespace AdventOfCode.Years.Year2022.Days.Day11;

public class Monkey
{

    private readonly Main m_Main;
    private readonly int m_MonkeyNumber;
    private readonly Queue<Item> m_Items;

    private readonly Func<Int256, Int256> m_WorryOperation;

    private readonly Func<Int256, Int256> m_BoredOperation;

    private readonly int m_TestModulo;

    private readonly int m_DestinationIfTrue;

    private readonly int m_DestinationIfFalse;

    private long m_ProcessedCount;
    
    public long ProcessedCount
    {
        get { return m_ProcessedCount; }
    }
    
    public int MonkeyNumber
    {
        get { return m_MonkeyNumber; }
    }

    public int TestModulo
    {
        get { return m_TestModulo; }
    }

    public long Product
    {
        get;
        set;
    }

    public Monkey(int monkeyNumber, Main main, Func<Int256, Int256> worryOperation, int divisibleBy, int destinationIfTrue,
                  int destinationIfFalse)
    {
        m_MonkeyNumber = monkeyNumber;
        m_BoredOperation = (i => i );
        m_Main = main;
        m_Items = new Queue<Item>();
        m_WorryOperation = worryOperation;
        m_TestModulo = divisibleBy;
        m_DestinationIfTrue = destinationIfTrue;
        m_DestinationIfFalse = destinationIfFalse;
        m_ProcessedCount = 0;
    }

    public void AddItem(Item item)
    {
        m_Items.Enqueue(item);
    }

    public void Process()
    {
        while (m_Items.Count > 0)
        {
            var item = m_Items.Dequeue();
            item.Worry = m_WorryOperation(item.Worry);
            item.Worry = m_BoredOperation(item.Worry);
            if (Product != 0)
                item.Worry = item.Worry % Product;
            int destination = item.Worry % m_TestModulo == 0 ? m_DestinationIfTrue : m_DestinationIfFalse;
            item.ThrowCount++;
            m_Main.GetMonkey(destination).AddItem(item);
            m_ProcessedCount++;
        }
    }

    private static bool DivisibleByTest(Int256 input, int divisor)
    {
        return input % divisor == 0;
    }

    public void PrintInfo()
    {
        var items = m_Items.ToArray();
        Console.Write($"Monkey {m_MonkeyNumber}:");
        Console.Write(string.Join(", ", items.Select(i => i.Worry)));
        Console.WriteLine();
    }
}

public static class MonkeyExtensions
{
    public static void AddItem(this Monkey extends, int worry)
    {
        extends.AddItem(new Item(worry));
    }
}