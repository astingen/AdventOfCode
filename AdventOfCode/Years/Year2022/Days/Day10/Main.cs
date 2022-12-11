using System.Runtime.InteropServices;

namespace AdventOfCode.Years.Year2022.Days.Day10;

public sealed class Main
{
    private static readonly int[] s_CheckCycles = new int[] {20, 60, 100, 140, 180, 220 };

    private readonly SortedDictionary<int, bool> m_CheckedCycles;

    /// <summary>
    /// Key: Cycle
    /// Value: Register value during cycle
    /// </summary>
    private readonly Dictionary<int,int> m_CycleRegisterValue;
    
    private int m_Register;
    private int m_CurrentCycle;
    private int m_SignalStrenght;

    public Main()
    {
        m_CurrentCycle = 0;
        m_Register = 1;
        m_SignalStrenght = 0;
        
        m_CheckedCycles = new SortedDictionary<int, bool>();
        foreach (int i in s_CheckCycles) 
            m_CheckedCycles.Add(i, false);

        m_CycleRegisterValue = new Dictionary<int, int>();
        // Add initial state
        //LogCycleRegister();


    }

    private void LogCycleRegister()
    {
        m_CycleRegisterValue[m_CurrentCycle] = m_Register;
    }


    public void Run()
    {
        Console.WriteLine("AdventOfCode 2022-12-10");
        
        var checkedCycles = new SortedDictionary<int, bool>();
        
        foreach (int i in s_CheckCycles)
            checkedCycles.Add(i, false);

        

        foreach (var instruction in Input.InputString)
        {
            int registerIncrement = 0;
            // Check instruction, increase cycle appropriately
            if (instruction.StartsWith("noop"))
            {
                m_CurrentCycle++;
                LogCycleRegister();
                
            }
            else if (instruction.StartsWith("addx"))
            {
                m_CurrentCycle++;
                LogCycleRegister();
                m_CurrentCycle++;
                LogCycleRegister();
                registerIncrement = Int32.Parse(instruction.Substring(5));
            }
            else
                throw new InvalidOperationException();
            
            // Check to see if we need to do anything here, before incrementing register
            int nextCheckCycle;
            try
            {
                nextCheckCycle = checkedCycles.First(kvp => !kvp.Value).Key;
            }
            catch (InvalidOperationException)
            {
                // No more to check
                nextCheckCycle = int.MaxValue;
            }
            
            if (m_CurrentCycle >= nextCheckCycle)
            {
                //increase signal strength
                m_SignalStrenght += m_Register * nextCheckCycle;
                checkedCycles[nextCheckCycle] = true;
            }
            
            //Increment register and continue
            m_Register += registerIncrement;
        }
        
        Console.WriteLine($"Signal Strength Sum: {m_SignalStrenght}");
    }

    public void PrintLines()
    {
        for (int i = 1; i <= 240; i++)
        {
            Console.Write(GetCharacterForCycle(i));
            if (GetPositionForCycle(i) == 39)
                Console.WriteLine();
        }
    }

    private string GetCharacterForCycle(int cycle)
    {
        int register;
        if (!m_CycleRegisterValue.TryGetValue(cycle, out register))
            throw new KeyNotFoundException();

        int position = GetPositionForCycle(cycle);

        if (position <= register + 1 && position >= register - 1)
            return "#";

        return ".";

    }

    private static int GetPositionForCycle(int cycle)
    {
        return (cycle - 1) % 40;
    }
}