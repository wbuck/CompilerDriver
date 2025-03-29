
using System.CommandLine;
using System.Diagnostics;
using CliWrap;
using CliWrap.Buffered;

var file = new Argument<string>
(
    "input",
    "The input file to compile"
);
file.AddValidator(arg =>
{
    if (!File.Exists(arg.GetValueForArgument(file)))
        arg.ErrorMessage = "File does not exist";
});

var lex = new Option<bool>
(
    "--lex",
    () => true,
    "Run the lexer but stop before parsing"
);

var parse = new Option<bool>
(
    "--parse",
    () => true,
    "Run the lexer and parser but stop before assembly generation"
);

var codegen = new Option<bool>
(
    "--codegen",
    () => true,
    "Run the lexer, parser, and assembly generation but stop before code generation"
);

var root = new RootCommand("Compiler Driver");
root.AddArgument(file);
root.AddOption(lex);
root.AddOption(parse);
root.AddOption(codegen);

root.SetHandler(async (ctx) =>
{
    var token = ctx.GetCancellationToken();
    var input = ctx.ParseResult.GetValueForArgument(file);

    if (ctx.ParseResult.GetValueForOption(lex))
    {
        if ((ctx.ExitCode = await PreprocessAsync(input, token)) != 0)        
            return;
    }
    
    await Task.CompletedTask;
});

await root.InvokeAsync(args);
return;

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






