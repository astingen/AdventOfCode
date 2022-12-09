using JetBrains.Annotations;

namespace AdventOfCode.Extensions;

public static class Extensions
{

    [NotNull]
    public static IEnumerable<T> Give<T>([CanBeNull] this T extends, int instances)
    {
        if (instances < 0)
            throw new ArgumentOutOfRangeException("instances");
        if (instances == 0)
            yield break;

        for (int i = 0; i < instances; i++)
            yield return extends;
    }
    public static int IncrementOrAddNewValue<TKey>(this Dictionary<TKey, int> extends, TKey item)
    {
        if (extends == null) throw new ArgumentNullException(nameof(extends));

        int value;
        if (extends.TryGetValue(item, out value))
            value++;
        else
            value = 1;

        extends[item] = value;
        return value;
    }
    
    public static int DecrementOrAddNewValue<TKey>(this Dictionary<TKey, int> extends, TKey item)
    {
        if (extends == null) throw new ArgumentNullException(nameof(extends));

        int value;
        if (extends.TryGetValue(item, out value))
            value--;
        else
            value = -1;

        extends[item] = value;
        return value;
    }

    public static int DecrementWithRemove<TKey>(this Dictionary<TKey, int> extends, TKey key)
    {
        int value;
        if (!extends.TryGetValue(key, out value))
            return 0;
        
        value--;
        if (value <= 0)
            extends.Remove(key);
        else
            extends[key] = value;

        return value;
    }

    public static TValue GetOrAddDefault<TKey, TValue>(this Dictionary<TKey, TValue> extends, TKey key)
    {
        return extends.GetOrAddNew(key, () => default);
    }

    /// <summary>
    /// If the key is present in the dictionary return the value, otherwise add a new value to the dictionary and return it.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="extends"></param>
    /// <param name="key"></param>
    /// <param name="valueFunc"></param>
    /// <returns></returns>
    [PublicAPI]
    public static TValue GetOrAddNew<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> extends,
                                                   [NotNull] TKey key,
                                                   [NotNull] Func<TValue> valueFunc)
    {
        if (extends == null)
            throw new ArgumentNullException("extends");

        // ReSharper disable CompareNonConstrainedGenericWithNull
        if (key == null)
            // ReSharper restore CompareNonConstrainedGenericWithNull
            throw new ArgumentNullException("key");

        if (valueFunc == null)
            throw new ArgumentNullException("valueFunc");

        TValue value;
        if (!extends.TryGetValue(key, out value))
        {
            value = valueFunc();
            extends.Add(key, value);
        }

        return value;
    }

    public static Dictionary<TInnerKey, TValue> GetOrAddNew<TOuterKey, TInnerKey, TValue>(
        [NotNull] this Dictionary<TOuterKey, Dictionary<TInnerKey, TValue>> extends, TOuterKey outerKey) 
        where TInnerKey : notnull 
        where TOuterKey : notnull
    {
        if (extends == null) throw new ArgumentNullException(nameof(extends));

        return extends.GetOrAddNew(outerKey, () => new Dictionary<TInnerKey, TValue>());
    }
    
    public static bool TryGetValue<TOuterKey, TInnerKey, TValue>(
        [NotNull] this Dictionary<TOuterKey, Dictionary<TInnerKey, TValue>> extends, TOuterKey outerKey, TInnerKey innerKey, out TValue value) 
        where TInnerKey : notnull 
        where TOuterKey : notnull
    {
        if (extends == null) throw new ArgumentNullException(nameof(extends));

        value = default;

        Dictionary<TInnerKey, TValue> innerDict;
        if (!extends.TryGetValue(outerKey, out innerDict))
            return false;
        
        return innerDict.TryGetValue(innerKey, out value);

    }
    
    public static bool SetValue<TOuterKey, TInnerKey, TValue>(
        [NotNull] this Dictionary<TOuterKey, Dictionary<TInnerKey, TValue>> extends, TOuterKey outerKey, TInnerKey innerKey, TValue value) 
        where TInnerKey : notnull 
        where TOuterKey : notnull
    {
        if (extends == null) throw new ArgumentNullException(nameof(extends));

        var innerDict = extends.GetOrAddNew(outerKey);

        var isAdded = !innerDict.ContainsKey(innerKey);

        innerDict[innerKey] = value;

        return isAdded;

    }
}