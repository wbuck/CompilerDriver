using System.CommandLine;
using System.CommandLine.Invocation;

namespace Compiler.Driver.Extensions;

public static class InvocationContextExtensions
{
    public static T? GetOption<T>(this InvocationContext ctx, Option<T> option) 
        => ctx.ParseResult.GetValueForOption(option);
    
    public static T GetOption<T>(this InvocationContext ctx, Option<T> option, T defaultValue) 
        => ctx.ParseResult.GetValueForOption(option) ?? defaultValue;
}