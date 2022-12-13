using System.Runtime.Intrinsics.X86;

namespace AdventOfCode.Years.Year2022.Days.Day12;

public class Main
{

    private readonly Dictionary<(int X, int Y), int> m_Elevations;
    private readonly Dictionary<(int X, int Y), int> m_MinimumSteps;

    private (int X, int Y) m_StartPoint;
    private (int X, int Y) m_EndPoint;

    private int m_MinimumToGround;

    public Main()
    {
        m_Elevations = new Dictionary<(int X, int Y), int>();
        m_MinimumSteps = new Dictionary<(int X, int Y), int>();
        m_MinimumToGround = int.MaxValue;
    }

    public void RunInstance()
    {
        int y = 0;
        foreach (var line in Input.InputString)
        {
            int x = 0;
            foreach (var elevation in line.ToCharArray())
            {
                if (elevation == 'S')
                {
                    m_StartPoint = (x, y);
                    m_Elevations.Add((x, y), ToElevationInt('a'));
                }
                else if (elevation == 'E')
                {
                    m_EndPoint = (x, y);
                    m_Elevations.Add((x, y), ToElevationInt('z'));
                    m_MinimumSteps.Add((x, y), 0);

                }
                else
                {
                    m_Elevations.Add((x, y), ToElevationInt(elevation));
                }

                x++;
            }

            y++;
        }
        
        TraversePath(m_EndPoint, 0);
        
        Console.WriteLine($"Minimum Steps:{m_MinimumSteps[m_StartPoint]}");
        Console.WriteLine($"Minimum to Ground:{m_MinimumToGround}");
    }

    private void TraversePath((int X, int Y) startLocation, int currentSteps)
    {
        currentSteps++;

        int minimumElevation = m_Elevations[startLocation] - 1;
        (int X, int Y) nextPoint;
        
        //Check each direction
        
        //Up
        nextPoint = (startLocation.X, startLocation.Y + 1);

        if (CheckLocationElevation(nextPoint, minimumElevation)) 
            CheckPoint(nextPoint, currentSteps);

        //Down
        nextPoint = (startLocation.X, startLocation.Y - 1);
        if (CheckLocationElevation(nextPoint, minimumElevation))
            CheckPoint(nextPoint, currentSteps);
        
        //Left
        nextPoint = (startLocation.X - 1, startLocation.Y );
        if (CheckLocationElevation(nextPoint, minimumElevation))
            CheckPoint(nextPoint, currentSteps);
        
        //Right
        nextPoint = (startLocation.X + 1, startLocation.Y );
        if (CheckLocationElevation(nextPoint, minimumElevation))
            CheckPoint(nextPoint, currentSteps);

    }

    /// <summary>
    /// Check to see if the given location exists
    /// and is less than or equal to the maximum elevation
    /// </summary>
    /// <param name="point"></param>
    /// <param name="maximumElevation"></param>
    /// <returns></returns>
    public bool CheckLocationElevation((int X, int Y) point, int minimumElevation)
    {
        int elevation;
        return m_Elevations.TryGetValue(point, out elevation) && elevation >= minimumElevation;
    }

    private void CheckPoint((int X, int Y) point, int steps)
    {
        int minimumSteps;
        if (m_MinimumSteps.TryGetValue(point, out minimumSteps) && minimumSteps <= steps) 
            return;
        m_MinimumSteps[point] = steps;
        if (m_Elevations[point] == 0)
        {
            if (steps < m_MinimumToGround)
                m_MinimumToGround = steps;
        }
        TraversePath(point, steps);
    }

    public static int ToElevationInt(char elevation)
    {
        return elevation - 0x61;
    }
    
    public static void Run()
    {
        var main = new Main();
        main.RunInstance();
    }
}