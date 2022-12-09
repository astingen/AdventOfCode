using AdventOfCode.Extensions;

namespace AdventOfCode.Years.Year2022.Days.Day08;

public class Main
{
    private int m_TotalRows;
    private int m_TotalColumns;
    private readonly Dictionary<int, Dictionary<int, int>> m_HeightMap;
    private readonly Dictionary<int, Dictionary<int, eDirections>> m_VisibilityMap;
    
    private readonly Dictionary<int, Dictionary<int, int>> m_ScoreUp;
    private readonly Dictionary<int, Dictionary<int, int>> m_ScoreDown;
    private readonly Dictionary<int, Dictionary<int, int>> m_ScoreLeft;
    private readonly Dictionary<int, Dictionary<int, int>> m_ScoreRight;

    [Flags]
    public enum eDirections
    {
        None = 0,
        Left = 1,
        Right = 2,
        Up = 4,
        Down = 8
    }

    public Main()
    {
        m_HeightMap = new Dictionary<int, Dictionary<int, int>>();
        m_VisibilityMap = new Dictionary<int, Dictionary<int, eDirections>>();
        m_ScoreUp = new Dictionary<int, Dictionary<int, int>>();
        m_ScoreDown = new Dictionary<int, Dictionary<int, int>>();
        m_ScoreLeft = new Dictionary<int, Dictionary<int, int>>();
        m_ScoreRight = new Dictionary<int, Dictionary<int, int>>();
    }

    public void Run()
    {
        var inputArray = Input.InputString.ToArray();

        m_TotalRows = inputArray.Length;
        m_TotalColumns = inputArray.First().Length;

        

        {
            int row = 0;
            // rows as lines
            foreach (var line in inputArray)
            {
                int column = 0;
                foreach (var item in line.ToCharArray())
                {
                    m_HeightMap.GetOrAddNew(row)[column] = int.Parse(item.ToString());
                    column++;
                }

                row++;
            }
        }

        int totalVisible = 0;
        // Check if visible:
        for (int row = 0 ; row < m_TotalRows; row++)
        {
            for (int column = 0; column < m_TotalColumns; column++)
            {
                var visible = IsTreeVisible(row, column);
                if (visible != eDirections.None)
                    totalVisible++;
                m_VisibilityMap.SetValue(row, column, visible);
            }
        }
        
        Console.WriteLine($"Total Visible Trees: {totalVisible}");
        
        int maxScore = 0;
        // Check For Score
        for (int row = 0 ; row < m_TotalRows; row++)
        {
            for (int column = 0; column < m_TotalColumns; column++)
            {
                var score = GetScenicScore(row, column);
                if (score > maxScore)
                    maxScore = score;
            }
        }
        
        Console.WriteLine($"Max Score: {maxScore}");
    }
    
    private int GetScenicScore(int row, int column)
    {
        int scoreLeft = 0;
        int scoreRight = 0;
        int scoreUp = 0;
        int scoreDown = 0;

        int height;
        if (!m_HeightMap.TryGetValue(row, column, out height))
            throw new KeyNotFoundException("Height Map Get");
                
        // check other trees (this is gonna be gross!)
        
        // Check to the left
        // column from position to 0
        for (int checkCol = column - 1; checkCol >= 0; checkCol--)
        {
            int checkHeight;
            if (!m_HeightMap.TryGetValue(row, checkCol, out checkHeight))
                throw new KeyNotFoundException("Height Map Check");
            
            //Increment immediately
            scoreLeft++;
            
            if (checkHeight >= height)
            {
                // No more scores!
                break;
            }
        }
        
        // Check to the right
        // column from position to total columns
        for (int checkCol = column + 1; checkCol < m_TotalColumns; checkCol++)
        {
            int checkHeight;
            if (!m_HeightMap.TryGetValue(row, checkCol, out checkHeight))
                throw new KeyNotFoundException("Height Map Check");

            scoreRight++;

            if (checkHeight >= height)
            {
                // Not visible from the right!
                break;
            }
        }
        
        // Check up
        // row from position to 0
        for (int checkRow = row - 1; checkRow >= 0; checkRow--)
        {
            int checkHeight;
            if (!m_HeightMap.TryGetValue(checkRow, column, out checkHeight))
                throw new KeyNotFoundException("Height Map Check");

            scoreUp++;
            
            if (checkHeight >= height)
            {
                // Not visible from the top!
                break;
            }
        }
        
        // Check down
        // row from position to total columns
        for (int checkRow = row + 1; checkRow < m_TotalRows; checkRow++)
        {
            int checkHeight;
            if (!m_HeightMap.TryGetValue(checkRow, column, out checkHeight))
                throw new KeyNotFoundException("Height Map Check");

            scoreDown++;
            
            if (checkHeight >= height)
            {
                // Not visible from the bottom!
                break;
            }
        }

        m_ScoreLeft.SetValue(row, column, scoreLeft);
        m_ScoreRight.SetValue(row, column, scoreRight);
        m_ScoreUp.SetValue(row, column, scoreUp);
        m_ScoreDown.SetValue(row, column, scoreDown);

        return scoreLeft * scoreRight * scoreUp * scoreDown;
    }

    private eDirections IsTreeVisible(int row, int column)
    {
        eDirections directions = eDirections.None;

        // Edge case of outer trees;
        if (row == 0 || column == 0 || row == m_TotalRows - 1 || column == m_TotalColumns - 1)
        {
            if (row == 0)
                directions = directions.SetFlags(eDirections.Up, true);
            if (column == 0)
                directions = directions.SetFlags(eDirections.Left, true);
            if (row == m_TotalRows - 1)
                directions = directions.SetFlags(eDirections.Down, true);
            if (column == m_TotalColumns - 1)
                directions = directions.SetFlags(eDirections.Right, true);
            
            // Something is always visible
            return directions;
        }
        
        int height;
        if (!m_HeightMap.TryGetValue(row, column, out height))
            throw new KeyNotFoundException("Height Map Get");
                
        // check other trees (this is gonna be gross!)
        
        // Check to the left
        // column from position to 0
        for (int checkCol = column - 1; checkCol >= 0; checkCol--)
        {
            int checkHeight;
            if (!m_HeightMap.TryGetValue(row, checkCol, out checkHeight))
                throw new KeyNotFoundException("Height Map Check");

            if (checkHeight >= height)
            {
                // Not visible from the left!
                break;
            }

            // Shortcut - if the tree beside isn't taller and visible in this direction, so are we. 
            eDirections checkVisibility;
            if (m_VisibilityMap.TryGetValue(row, checkCol, out checkVisibility) && checkVisibility.HasFlag(eDirections.Left))
            {
                directions = directions.SetFlags(eDirections.Left, true);
                break;
            }

            // If we get to the end, we are visible;
            if (checkCol <= 0)
                directions = directions.SetFlags(eDirections.Left, true);
        }
        
        // Check to the right
        // column from position to total columns
        for (int checkCol = column + 1; checkCol < m_TotalColumns; checkCol++)
        {
            int checkHeight;
            if (!m_HeightMap.TryGetValue(row, checkCol, out checkHeight))
                throw new KeyNotFoundException("Height Map Check");

            if (checkHeight >= height)
            {
                // Not visible from the right!
                break;
            }

            // Shortcut - if the tree beside is shorter and visible in this direction, so are we. 
            eDirections checkVisibility;
            if (m_VisibilityMap.TryGetValue(row, checkCol, out checkVisibility) && checkVisibility.HasFlag(eDirections.Right))
            {
                directions = directions.SetFlags(eDirections.Right, true);
                break;
            }

            // If we get to the end, we are visible;
            if (checkCol >= m_TotalColumns - 1)
                directions = directions.SetFlags(eDirections.Right, true);
        }
        
        // Check up
        // row from position to 0
        for (int checkRow = row - 1; checkRow >= 0; checkRow--)
        {
            int checkHeight;
            if (!m_HeightMap.TryGetValue(checkRow, column, out checkHeight))
                throw new KeyNotFoundException("Height Map Check");

            if (checkHeight >= height)
            {
                // Not visible from the top!
                break;
            }

            // Shortcut - if the tree beside isn't taller and visible in this direction, so are we. 
            eDirections checkVisibility;
            if (m_VisibilityMap.TryGetValue(checkRow, column, out checkVisibility) && checkVisibility.HasFlag(eDirections.Up))
            {
                directions = directions.SetFlags(eDirections.Up, true);
                break;
            }

            // If we get to the end, we are visible;
            if (checkRow <= 0)
                directions = directions.SetFlags(eDirections.Up, true);
        }
        
        // Check down
        // row from position to total columns
        for (int checkRow = row + 1; checkRow < m_TotalRows; checkRow++)
        {
            int checkHeight;
            if (!m_HeightMap.TryGetValue(checkRow, column, out checkHeight))
                throw new KeyNotFoundException("Height Map Check");

            if (checkHeight >= height)
            {
                // Not visible from the bottom!
                break;
            }

            // Shortcut - if the tree beside is shorter and visible in this direction, so are we. 
            eDirections checkVisibility;
            if (m_VisibilityMap.TryGetValue(checkRow, column, out checkVisibility) && checkVisibility.HasFlag(eDirections.Down))
            {
                directions = directions.SetFlags(eDirections.Down, true);
                break;
            }

            // If we get to the end, we are visible;
            if (checkRow >= m_TotalRows - 1)
                directions = directions.SetFlags(eDirections.Down, true);
        }

        return directions;
    }

    public void PrintHeightMap()
    {
        for (int row = 0 ; row < m_TotalRows; row++)
        {
            for (int column = 0; column < m_TotalColumns; column++)
            {
                int height;
                m_HeightMap.TryGetValue(row, column, out height);
                Console.Write(height.ToString());
            }

            Console.WriteLine();
        }
    }

    public void PrintVisibilityMap()
    {
        for (int row = 0 ; row < m_TotalRows; row++)
        {
            for (int column = 0; column < m_TotalColumns; column++)
            {
                eDirections visibility;
                m_VisibilityMap.TryGetValue(row, column, out visibility);
                bool visible = visibility != eDirections.None;
                Console.Write(visible?"Y":"N");
            }

            Console.WriteLine();
        }
    }
    
    public void PrintScoreUp()
    {
        Console.WriteLine("Score Up:");
        PrintScore(m_ScoreUp.TryGetValue);
        Console.WriteLine();
    }

    public void PrintScoreDown()
    {
        Console.WriteLine("Score Down:");
        PrintScore(m_ScoreDown.TryGetValue);
        Console.WriteLine();
    }

    public void PrintScoreLeft()
    {
        Console.WriteLine("Score Left:");
        PrintScore(m_ScoreLeft.TryGetValue);
        Console.WriteLine();
    }

    public void PrintScoreRight()
    {
        Console.WriteLine("Score Right:");
        PrintScore(m_ScoreRight.TryGetValue);
        Console.WriteLine();
    }

    private delegate bool GetScore(int row, int column, out int score);
    
    private void PrintScore(GetScore getScore)
    {
        for (int row = 0 ; row < m_TotalRows; row++)
        {
            for (int column = 0; column < m_TotalColumns; column++)
            {
                int score;
                if (getScore(row, column, out score))
                    Console.Write(GetScoreText(score));
                else
                    Console.Write("X");
            }

            Console.WriteLine();
        }
    }

    private string GetScoreText(int score)
    {
        return score >= 10 ? "+" : score.ToString();
    }


}