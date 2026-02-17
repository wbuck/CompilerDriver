using System.Numerics;

namespace Compiler.Common.Symbols;

public sealed record Initial<T>(T Value) : StaticInitValue, IConstantInit where T: INumber<T>;