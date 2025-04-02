using System.Text.Json.Serialization;
using Compiler.Common.Nodes;

namespace Compiler.Common.Contexts;

[JsonSerializable(typeof(Program))]
[JsonSourceGenerationOptions(WriteIndented = true, UseStringEnumConverter = true)]
public partial class ProgramContext : JsonSerializerContext { }

[JsonSerializable(typeof(Function))]
[JsonSourceGenerationOptions(WriteIndented = true, UseStringEnumConverter = true)]
public partial class FunctionContext : JsonSerializerContext { }