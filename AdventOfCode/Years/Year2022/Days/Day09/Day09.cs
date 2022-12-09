using AdventOfCode.Extensions;

namespace AdventOfCode.Years.Year2022.Days.Day09;


public class Day09
{
    private const int TAILS = 9;

    private (int X, int Y) m_HeadPosition;
    private readonly Dictionary<int, (int X, int Y)> m_TailPositions;

    /// <summary>
    /// Number of times tail was in X/Y position
    /// Dictionary(X, Dictionary(Y, Count))
    /// </summary>
    private readonly Dictionary<int, Dictionary<int, int>> m_TailHistory;

    private (int X, int Y) HeadPosition
    {
        get { return m_HeadPosition; }
    }

    public Day09()
    {
        m_TailHistory = new Dictionary<int, Dictionary<int, int>>();
        m_TailPositions = new Dictionary<int, (int X, int Y)>();
        

    }

    public void Run()
    {
        m_HeadPosition = (0, 0);
        for (int i = 0; i < TAILS; i++)
            m_TailPositions[i] = (0, 0);

        foreach (var row in Input.InputString)
        {
            foreach (var headMovement in GetMovementsForRow(row))
            {
                MoveHead(headMovement);
                
                // Move each tail in series
                // Start with the head position as the connectedPosition;
                var connectedPosition = HeadPosition;
                for (int tail = 0; tail < TAILS; tail++)
                {
                    var movement = GetTailMovement(connectedPosition, GetTailPosition(tail));

                    connectedPosition = MoveTail(tail, movement);
                }
                //save last tail history
                LogTailPosition(connectedPosition);

            }
        }

        var totalPositions = m_TailHistory.Select(y => y.Value.Count).Sum();
        
        Console.WriteLine($"Total Tail Positions: {totalPositions}");

    }

    /// <summary>
    /// Logs how many times the tail has touched each position
    /// </summary>
    /// <param name="tailPosition"></param>
    private void LogTailPosition((int X, int Y) tailPosition)
    {
        int count;
        if (!m_TailHistory.TryGetValue(tailPosition.X, tailPosition.Y, out count))
            count = 0;

        count++;
        m_TailHistory.SetValue(tailPosition.X, tailPosition.Y, count);
    }

    /// <summary>
    /// Translates row of data into enumeration of movements
    /// </summary>
    /// <param name="row"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private IEnumerable<(int X, int Y)> GetMovementsForRow(string row)
    {
        int iterations = int.Parse(row.Substring(2));
        char direction = row[0];

        switch (direction)
        {
            case 'U':
                return (1, 0).Give(iterations);
            case 'D':
                return (-1, 0).Give(iterations);
            case 'R':
                return (0, 1).Give(iterations);
            case 'L':
                return (0, -1).Give(iterations);
            default:
                throw new ArgumentOutOfRangeException("row", "First Character out of range");
        }
        
    }

    /// <summary>
    /// Moves head
    /// </summary>
    /// <param name="movement">relative</param>
    /// <returns>absolute new position</returns>
    private (int X, int Y) MoveHead((int X, int Y) movement)
    {
        m_HeadPosition = Add(m_HeadPosition, movement);
        return m_HeadPosition;
    }

    /// <summary>
    /// Moves tail
    /// </summary>
    /// <param name="number"></param>
    /// <param name="movement">relative</param>
    /// <returns>absolute new position</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private (int X, int Y) MoveTail(int number, (int X, int Y) movement)
    {
        var position = Add(GetTailPosition(number), movement);
        m_TailPositions[number] = position;
        return position;
    }

    /// <summary>
    /// GetsTailPosition
    /// </summary>
    /// <param name="number"></param>
    /// <returns>absolute</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private (int X, int Y) GetTailPosition(int number)
    {
        (int x, int y) position;
        if (m_TailPositions.TryGetValue(number, out position))
            return position;

        throw new ArgumentOutOfRangeException("number");
    }

    /// <summary>
    /// Checks if the head and tail are touching
    /// </summary>
    /// <param name="head">absolute</param>
    /// <param name="tail">absolute</param>
    /// <returns></returns>
    private static bool AreHeadAndTailTouching((int X, int Y) head, (int X, int Y) tail)
    {
        if (tail.X > head.X + 1 || tail.X < head.X - 1)
            return false;
        if (tail.Y > head.Y + 1 || tail.Y < head.Y - 1)
            return false;
        return true;
    }

    /// <summary>
    /// Gets relative tail movement based on head/tail absolute positions
    /// </summary>
    /// <param name="headPosition">absolute</param>
    /// <param name="tailPosition">absolute</param>
    /// <returns>relative</returns>
    private static (int X, int Y) GetTailMovement((int X, int Y) headPosition, (int X, int Y) tailPosition)
    {
        if (AreHeadAndTailTouching(headPosition, tailPosition))
            return (0, 0);
        
        bool right = headPosition.X > tailPosition.X;
        bool up = headPosition.Y > tailPosition.Y;

        // Same Column
        if (headPosition.X == tailPosition.X)
        {
            return (0, up ? 1 : -1);
        }
        
        // Same Row
        if (headPosition.Y == tailPosition.Y)
        { 
            return (right ? 1 : -1, 0);
        }

        return (right ? 1 : -1, up ? 1 : -1);
    }
    
    private static (int X, int Y) Add((int X, int Y) first, (int X, int Y) second)
    {
        return (first.X + second.X, first.Y + second.Y);
    }
}