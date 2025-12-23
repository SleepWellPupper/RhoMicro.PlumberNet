// SPDX-License-Identifier: MPL-2.0

namespace RhoMicro.PlumberNet.Generator;

using CodeAnalysis.Lyra;

readonly record struct ArityDependentList(
    Int32 Arity,
    Action<Int32, Int32, CSharpSourceBuilder, CancellationToken> Expression)
    : ICSharpSourceComponent
{
    public static ArityDependentList CreateTypeParameters(Int32 arity) =>
        new(arity, static (_, i, b, ct) =>
        {
            ct.ThrowIfCancellationRequested();

            b.Append($"T{i}");
        });

    public static ArityDependentList CreateArgumentsArgumentAccessors(Int32 arity) =>
        new(arity, static (_, i, b, ct) =>
        {
            ct.ThrowIfCancellationRequested();

            b.Append($"arguments.Argument{i}");
        });

    public static ArityDependentList CreateAdditionalArgumentsArgumentAccessors(Int32 arity) =>
        new(arity, static (_, i, b, ct) =>
        {
            ct.ThrowIfCancellationRequested();

            b.Append($"additionalArguments.Argument{i}");
        });

    public void AppendTo(CSharpSourceBuilder builder, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        for (var i = 1; i <= Arity; i++)
        {
            ct.ThrowIfCancellationRequested();

            if (i is not 1)
            {
                builder.Append(", ");
            }

            Expression.Invoke(Arity, i, builder, ct);
        }
    }
}
