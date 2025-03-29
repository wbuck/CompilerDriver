
using System.CommandLine;
using System.Diagnostics;
using CliWrap;
using CliWrap.Buffered;
using CompilerDriver.Extensions;

var file = new Argument<string>
(
    "input",
    "The input file to compile"
);
file.AddValidator(arg =>
{
    if (!File.Exists(arg.GetValueForArgument(file)))
        arg.ErrorMessage = $"File does not exist";
});

var output = new Option<string>
(
    ["-o", "--output"],
    "The output file to generate"
);

var lex = new Option<bool>
(
    "--lex",
    "Run the lexer but stop before parsing"
);

var parse = new Option<bool>
(
    "--parse",
    "Run the lexer and parser but stop before assembly generation"
);

var codegen = new Option<bool>
(
    "--codegen",
    "Run the lexer, parser, and assembly generation but stop before code generation"
);

var root = new RootCommand("Compiler Driver");
root.AddArgument(file);
root.AddOption(lex);
root.AddOption(parse);
root.AddOption(codegen);
root.AddOption(output);

root.SetHandler(async (ctx) =>
{
    var token = ctx.GetCancellationToken();
    var input = ctx.ParseResult.GetValueForArgument(file);
    
    if ((ctx.ExitCode = await PreprocessAsync(input, token)) != 0)        
        return;
    
    if ((ctx.ExitCode = await CompileAsync(input, token)) != 0)
        return;
   
    if ((ctx.ExitCode = await AssembleAndLinkAsync(input, ctx.GetOption(output), token)) != 0)
        return;
});

await root.InvokeAsync(args);
return;

static async Task<int> AssembleAndLinkAsync(string assemblyFile, string? output = null, CancellationToken token = default)
{
    output ??= Path.ChangeExtension(assemblyFile, ".o");
    
    var result = await Cli.Wrap("gcc")
        .WithArguments(args => args
            .Add(assemblyFile)
            .Add("-o").Add(output))
        .WithStandardOutputPipe(PipeTarget.ToDelegate(Console.WriteLine))
        .WithStandardErrorPipe(PipeTarget.ToDelegate(WriteError))
        .WithValidation(CommandResultValidation.None)
        .ExecuteAsync(token);

    return result.ExitCode;
}

static Task<int> CompileAsync(string file, CancellationToken token = default)
{
    throw new NotImplementedException();
}

static async Task<int> PreprocessAsync(string file, CancellationToken token = default)
{
    var result = await Cli.Wrap("gcc")
        .WithArguments(args => args
            .Add("-E")
            .Add("-P")
            .Add(file)
            .Add("-o").Add($"{file}.i"))
        .WithStandardOutputPipe(PipeTarget.ToDelegate(Console.WriteLine))
        .WithStandardErrorPipe(PipeTarget.ToDelegate(WriteError))
        .WithValidation(CommandResultValidation.None)
        .ExecuteAsync(token);

    return result.ExitCode;
}

static void WriteError(string message)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.Error.WriteLine(message);
    Console.ResetColor();
}






