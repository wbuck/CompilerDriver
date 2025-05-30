using System.Runtime.InteropServices;

namespace Compiler.Common.Extensions;

public static class DictionaryExtensions
{
    public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key,
        Func<TKey, TValue> valueFactory) where TKey : notnull
    {
        ref var value = ref CollectionsMarshal.GetValueRefOrAddDefault(dict, key, out var exists);
        return exists ? value! : value = valueFactory(key);
    }
}