namespace AdventOfCode.Years.Year2022.Days.Day10;

public sealed class Main
{
    private static readonly int[] s_CheckCycles = new int[] {20, 60, 100, 140, 180, 220 };

    public static void Run()
    {
        var checkedCycles = new SortedDictionary<int, bool>();
        foreach (int i in s_CheckCycles)
            checkedCycles.Add(i, false);

        int currentCycle = 0;
        int register = 1;
        int signalStrenght = 0;

        foreach (var instruction in Input.InputString)
        {
            int registerIncrement = 0;
            // Check instruction, increase cycle appropriately
            if (instruction.StartsWith("noop"))
                currentCycle++;
            else if (instruction.StartsWith("addx"))
            {
                currentCycle += 2;
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
                Console.WriteLine("Ran out of check cycles");
                break;
            }
            
            if (currentCycle >= nextCheckCycle)
            {
                //increase signal strength
                signalStrenght += register * nextCheckCycle;
                checkedCycles[nextCheckCycle] = true;
            }
            
            //Increment register and continue
            register += registerIncrement;
        }
        
        Console.WriteLine($"Signal Strenngth Sum: {signalStrenght}");
    }
}