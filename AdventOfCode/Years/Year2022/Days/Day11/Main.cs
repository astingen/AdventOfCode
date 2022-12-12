using System.Numerics;

namespace AdventOfCode.Years.Year2022.Days.Day11;

public class Main
{
    private readonly SortedDictionary<int, Monkey> m_Monkeys;
    
    public IEnumerable<Monkey> Monkeys
    {
        get { return m_Monkeys.Values.ToArray(); }
    }

    public Main()
    {
        m_Monkeys = new SortedDictionary<int, Monkey>();
    }

    public Monkey GetMonkey(int number)
    {
        return m_Monkeys[number];
    }

    public void Process()
    {
        foreach (var monkey in m_Monkeys.Values)
        {
            monkey.Process();
        }
    }

    public long GetMonkeyBusiness()
    {
        return m_Monkeys.Values.Select(m=> m.ProcessedCount).OrderByDescending(i => i).Take(2).Aggregate((n, o) => n * o);
    }

    public void Process(int rounds)
    {
        for(int i = 0 ; i < rounds; i++)
            Process();
    }

    public void AddMonkey(Monkey monkey)
    {
        m_Monkeys.Add(monkey.MonkeyNumber, monkey);
        long factor = m_Monkeys.Values.Select(m => m.TestModulo).Aggregate((a, b) => a * b);
        foreach (var m in m_Monkeys.Values)
        {
            m.Product = factor;
        }
    }

    public static void Run()
    {
        var main = new Main();

        var monkey0 = new Monkey(0, main, i => i * 2, 17, 2, 5);
        AddItems(monkey0, "96, 60, 68, 91, 83, 57, 85");
        
        var monkey1 = new Monkey(1, main, i => i + 3, 13, 7, 4);
        AddItems(monkey1, "75, 78, 68, 81, 73, 99");

        var monkey2 = new Monkey(2, main, i => i + 6, 19, 6, 5);
        AddItems(monkey2, "69, 86, 67, 55, 96, 69, 94, 85");

        var monkey3 = new Monkey(3, main, i => i + 5, 7, 7, 1);
        AddItems(monkey3, "88, 75, 74, 98, 80");

        var monkey4 = new Monkey(4, main, i => i + 8, 11, 0, 2);
        AddItems(monkey4, "82");

        var monkey5 = new Monkey(5, main, i => i * 5, 3, 6, 3);
        AddItems(monkey5, "72, 92, 92");

        var monkey6 = new Monkey(6, main, i => i * i, 2, 3, 1);
        AddItems(monkey6, "74, 61");

        var monkey7 = new Monkey(7, main, i => i + 4, 5, 4, 0);
        AddItems(monkey7, "76, 86, 83, 55");
        
        main.AddMonkey(monkey0);
        main.AddMonkey(monkey1);
        main.AddMonkey(monkey2);
        main.AddMonkey(monkey3);
        main.AddMonkey(monkey4);
        main.AddMonkey(monkey5);
        main.AddMonkey(monkey6);
        main.AddMonkey(monkey7);

        main.Process(10000);
        
        foreach (var monkey in main.Monkeys)
        {
            Console.WriteLine($"Monkey {monkey.MonkeyNumber} inspected items {monkey.ProcessedCount}");
        }

        Console.WriteLine($"Monkey Business = {main.GetMonkeyBusiness()}");
    }

    private const int ROUNDS = 20;

    public static void AddItems(Monkey monkey, string items)
    {
        foreach (string s in items.Split(','))
        {
            monkey.AddItem(int.Parse(s));
        }
    }
    
    
}