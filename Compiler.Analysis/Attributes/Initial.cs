using System.Numerics;

namespace Compiler.Analysis.Attributes;

public sealed record Initial<T>(T Value) : StaticInitValue, IConstantInit where T: INumber<T>;