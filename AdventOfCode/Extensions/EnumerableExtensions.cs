namespace AdventOfCode.Extensions;

public static class EnumerableExtensions
{
    public static Dictionary<int, T> ToIndexedDictionary<T>(this IEnumerable<T> extends)
    {
        if (extends == null) throw new ArgumentNullException(nameof(extends));
        
        return extends.ToIndexedDictionary(0);
    }
    
    public static Dictionary<int, T> ToIndexedDictionary<T>(this IEnumerable<T> extends, int indexStart)
    {
        if (extends == null) throw new ArgumentNullException(nameof(extends));
        
        int i = indexStart;
        Dictionary<int, T> dictionary = new Dictionary<int, T>();
        foreach (T item in extends)
        {
            dictionary.Add(i, item);
            i++;
        }

        return dictionary;
    }
}