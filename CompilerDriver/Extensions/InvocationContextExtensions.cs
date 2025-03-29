using System.CommandLine;
using System.CommandLine.Invocation;

namespace CompilerDriver.Extensions;

public static class InvocationContextExtensions
{
    public static T? GetOption<T>(this InvocationContext ctx, Option<T> option) 
        => ctx.ParseResult.GetValueForOption(option);
}