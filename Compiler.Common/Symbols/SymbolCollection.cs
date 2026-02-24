using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Compiler.Common.Symbols;

public static class SymbolCollection
{
    public static readonly ConcurrentDictionary<string, IEntry> Symbols = new();
    
    public static bool TryAdd(string name, IEntry entry) 
        => Symbols.TryAdd(name, entry);
    
    public static bool TryGetValue(string name, [NotNullWhen(true)] out IEntry? entry) 
        => Symbols.TryGetValue(name, out entry);
    
    public static IEntry? GetValueOrDefault(string name) 
        => Symbols.GetValueOrDefault(name);
    
    public static IEnumerable<IEntry> Values => Symbols.Values;
    
    public static int Count => Symbols.Count;
    
    public static void Clear() => Symbols.Clear();
    
    public static bool ContainsKey(string name) => Symbols.ContainsKey(name);
    
    public static bool TryRemove(string name, [NotNullWhen(true)] out IEntry? entry) 
        => Symbols.TryRemove(name, out entry);
    
    public static IEnumerator<IEntry> GetEnumerator() => 
        Symbols.Values.GetEnumerator();
    
    public static IEntry AddOrUpdate(string name, IEntry entry, Func<string, IEntry, IEntry> updateValueFactory) 
        => Symbols.AddOrUpdate(name, entry, updateValueFactory);
    
    public static IEntry AddOrUpdate(string name, Func<string, IEntry> addValueFactory, Func<string, IEntry, IEntry> updateValueFactory) 
        => Symbols.AddOrUpdate(name, addValueFactory, updateValueFactory);
    
    public static IEntry GetOrAdd(string name, Func<string, IEntry> valueFactory) 
        => Symbols.GetOrAdd(name, valueFactory); 
    
    public static IEntry Get(string name) 
        => Symbols[name];

    public static TEntry Get<TEntry>(string name) where TEntry : IEntry
        => (TEntry)Symbols[name];

    public static void Add(string key, IEntry entry)
        => Symbols[key] = entry;
    
    public static bool IsType<TType>(string name) where TType: IType
        => Symbols.TryGetValue(name, out var entry) && entry.Type is TType;
}