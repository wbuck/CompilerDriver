using System.CommandLine;
using System.Diagnostics.CodeAnalysis;
using CliWrap;
using Compiler.Common.Ast;
using Compiler.Common.Stages;
using Compiler.Driver;
using Compiler.Driver.Extensions;

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
var validate = new Option<bool>
(
    "--validate",
    "Run the lexer, parser and semantic validation but stop before assembly generation"
);
var tacky = new Option<bool>
(
    name: "--tacky",
    description: "Run the lexer, parser, and tacky generator but stop before code generation"   
);

var codegen = new Option<bool>
(
    "--codegen",
    "Run the lexer, parser, and assembly generation but stop before code generation"
);

var asm = new Option<bool>
(
    ["--assembly", "-S"], 
    "Generate assembly code"
);

var root = new RootCommand("Compiler Driver");
root.AddArgument(file);
root.AddOption(lex);
root.AddOption(parse);
root.AddOption(validate);
root.AddOption(tacky);
root.AddOption(codegen);
root.AddOption(output);

root.SetHandler(async (ctx) =>
{
    var token = ctx.GetCancellationToken();
    var input = ctx.ParseResult.GetValueForArgument(file);

    var result = await PreprocessAsync(input, token);
    if (!result.IsSuccess)
    {
        ctx.ExitCode = result;
        return;
    }
    
    var source = result.Value;
    var content = await File.ReadAllTextAsync(source, token);
    
    try
    {               
        if (!Lexer.TryTokenize(content, out var tokens))
        {
            ctx.ExitCode = 1;
            return;
        }
        if (ctx.GetOption(lex, false))
        {
            ctx.ExitCode = 0;
            return;
        }
    
        if (!Parser.TryParse(tokens, content.AsMemory(), out var node))
        {
            ctx.ExitCode = 1;
            return;
        }        
        if (ctx.GetOption(parse, false))
        {
            ctx.ExitCode = 0;
            return;
        }
        if (!SemanticValidator.TryValidate(node, out var analyzed))
        {
            ctx.ExitCode = 1;
            return;
        }        
        if (ctx.GetOption(validate, false))
        {
            ctx.ExitCode = 0;
            return;
        }
        if (!TackyGenerator.TryGenerate(node, out var tackyProgram))
        {
            ctx.ExitCode = 1;
            return;
        }
        if (ctx.GetOption(tacky, false))
        {
            ctx.ExitCode = 0;
            return;
        }

        if (!Generator.TryGenerate(tackyProgram, out var program))
        {
            ctx.ExitCode = 1;
            return;
        }

        if (ctx.GetOption(codegen, false))
        {
            ctx.ExitCode = 0;
            return;
        }
        
        if (!Emitter.TryEmit(program, out var compiled))
        {
            ctx.ExitCode = 0;
            return;
        }
        
        var path = Path.ChangeExtension(source, "s");        
        await File.WriteAllTextAsync(path, compiled, token);
        
        if (ctx.GetOption(asm, false))
        {
            ctx.ExitCode = 0;
            return;
        }
        
        if ((ctx.ExitCode = await AssembleAndLinkAsync(result.Value, ctx.GetOption(output), token)) != 0)
            return;
    }
    finally
    {
        if (!ctx.GetOption(asm, false))
            File.Delete(source);
    } 
});

return await root.InvokeAsync(args);

static async Task<int> AssembleAndLinkAsync(string assembly, string? output = null, CancellationToken token = default)
{
    output ??= Path.Combine
    (
        Path.GetDirectoryName(assembly) ?? string.Empty, 
        Path.GetFileNameWithoutExtension(assembly)
    );
        
    var result = await Cli.Wrap("gcc")
        .WithArguments(args => args
            .Add("-arch").Add("x86_64")            
            .Add(assembly)
            .Add("-o").Add(output))
        .WithStandardOutputPipe(PipeTarget.ToDelegate(Console.WriteLine))
        .WithStandardErrorPipe(PipeTarget.ToDelegate(WriteError))
        .WithValidation(CommandResultValidation.None)
        .ExecuteAsync(token);
    
    File.Delete(assembly);
    return result.ExitCode;
}

static async Task<Result<string>> PreprocessAsync(string file, CancellationToken token = default)
{
    var output = Path.ChangeExtension(file, ".i");
    var result = await Cli.Wrap("gcc")
        .WithArguments(args => args
            .Add("-E")
            .Add("-P")
            .Add(file)
            .Add("-o").Add(output))
        .WithStandardOutputPipe(PipeTarget.ToDelegate(Console.WriteLine))
        .WithStandardErrorPipe(PipeTarget.ToDelegate(WriteError))
        .WithValidation(CommandResultValidation.None)
        .ExecuteAsync(token);

    return new(result.ExitCode, output);
}

static void WriteError(string message)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.Error.WriteLine(message);
    Console.ResetColor();
}

namespace Compiler.Driver
{
    internal record Result<T>(int Code, T? Value = default)
    {
        private const int Success = 0;
    
        [MemberNotNullWhen(true, nameof(Value))]
        public bool IsSuccess => Code == Success;
        public T? Value { get; } = Value;
        public int Code { get; } = Code;
        public static implicit operator int(Result<T> result) => result.Code;
        public static implicit operator bool(Result<T> result) => result.Code == Success;
    }
}






