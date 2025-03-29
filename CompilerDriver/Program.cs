
using System.CommandLine;


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
root.AddOption(lex);
root.AddOption(parse);
root.AddOption(codegen);

root.SetHandler(static async (lex, parse, codegen) =>
{
    
     await Task.CompletedTask;
}, lex, parse, codegen);

await root.InvokeAsync(args);






