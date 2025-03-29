
using System.CommandLine;
using System.Diagnostics;
using CliWrap;

var file = new Argument<string>
(
    "input",
    "The input file to compile"
);

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
        Debugger.Break();
    }
    
    await Task.CompletedTask;
});

await root.InvokeAsync(args);

static Task PreprocessAsync(string file, CancellationToken token = default)
{
    return Cli.Wrap("gcc")
        .WithArguments([
            "-E",
            "-P",
            "INPUT_FILE",
            $"-o INPUT_FILE.i"
        ])
        .ExecuteAsync(token);
}






