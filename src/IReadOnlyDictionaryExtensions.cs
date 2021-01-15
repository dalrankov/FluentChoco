using System.Collections.Generic;

namespace FluentChoco
{
    static class IReadOnlyDictionaryExtensions
    {
        internal static TValue GetValueOrDefault<TKey, TValue>(
            this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.TryGetValue(key, out TValue value) ? value : default;
        }
    }
}