using System.Runtime.InteropServices;

namespace Compiler.Common.Extensions;

public static class DictionaryExtensions
{
    extension<TKey, TValue>(Dictionary<TKey, TValue> dict) where TKey : notnull
    {
        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            ref var value = ref CollectionsMarshal.GetValueRefOrAddDefault(dict, key, out var exists);
            return exists ? value! : value = valueFactory(key);
        }

        public TValue AddOrUpdate(TKey key, Func<TKey, TValue> valueFactory, Func<TKey, TValue, TValue> updateValueFactory)
        {
            ref var item = ref CollectionsMarshal.GetValueRefOrAddDefault(dict, key, out var exists);
            if (!exists) return item = valueFactory(key);
            
            return item = updateValueFactory(key, item!);
        }
    }
}