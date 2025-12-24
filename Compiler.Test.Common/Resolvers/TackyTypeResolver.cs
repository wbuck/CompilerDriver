using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Compiler.Tacky.Tac;

namespace Compiler.Test.Common.Resolvers;

public static class TackyTypeResolver
{
    public static void AddPolymorphicTypeInfo<TInterface>(JsonTypeInfo info)
    {
        var interfaceType = typeof(TInterface);
        if (interfaceType != info.Type) 
            return;
        
        info.PolymorphismOptions = new JsonPolymorphismOptions
        {
            UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
        };
        var types = interfaceType
            .Assembly
            .GetTypes()
            .Where(t => interfaceType.IsAssignableFrom(t) && t is { IsInterface: false })
            .ToList();

        if (types.Contains(typeof(TackyConstant<>)))
        {
            types.Remove(typeof(TackyConstant<>));
            types.Add(typeof(TackyConstant<int>));
            types.Add(typeof(TackyConstant<double>));
        }

        foreach (var type in types.Select(t => new JsonDerivedType(t, GetName(t))))
            info.PolymorphismOptions.DerivedTypes.Add(type);      
    }
    
    private static string GetName(Type type)
    {
        if (!type.IsGenericType)
            return type.Name;
            
        var genericType = type.GenericTypeArguments[0];

        var name = type.Name.AsSpan();
        name = name[..name.IndexOf('`')];
        var test = $"{name}[{genericType.Name}]";
        return test;
    }
}