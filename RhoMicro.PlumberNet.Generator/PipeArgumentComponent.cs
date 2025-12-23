// SPDX-License-Identifier: MPL-2.0

namespace RhoMicro.PlumberNet.Generator;

using CodeAnalysis.Lyra;
using static CodeAnalysis.Lyra.ComponentFactory;
using static CodeAnalysis.Lyra.ComponentFactory.Docs;

readonly record struct PipeArgumentComponent(Int32 Arity) : ICSharpSourceComponent
{
    public void AppendTo(CSharpSourceBuilder builder, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        var typeArguments = ArityDependentList.CreateTypeParameters(Arity);
        var parameters = new ArityDependentList(Arity, static (_, i, b, ct) =>
        {
            ct.ThrowIfCancellationRequested();

            b.Append($"T{i} Argument{i}");
        });
        var arguments = ArityDependentList.CreateArgumentsArgumentAccessors(Arity);
        var tupleMembers = new ArityDependentList(Arity, static (a, i, b, ct) =>
        {
            ct.ThrowIfCancellationRequested();

            if (a is 1)
            {
                b.Append($"T{i}");
            }
            else
            {
                b.Append($"T{i} value{i}");
            }
        });
        var tupleOpenParenthesis = Arity is 1 ? String.Empty : "(";
        var tupleClosingParenthesis = Arity is 1 ? String.Empty : ")";

        builder.Append($$"""
                         {{Summary("Represents a set of arguments to be passed through a pipe.")}}
                         public readonly record struct PipeArguments<{{typeArguments}}>({{parameters}})
                         {
                             /// {{Summary("Terminates a pipe, allowing access to the final arguments outside of the pipe.")}}
                             /// {{Param("arguments", "The final arguments to allow access to outside of the pipe.")}}
                             /// {{Param("pipeTerminator", "The pipe terminator demarking the ending of the pipe.")}}
                             /// {{Returns("The values of the arguments.")}}
                             public static {{tupleOpenParenthesis}}{{tupleMembers}}{{tupleClosingParenthesis}} operator |(PipeArguments<{{typeArguments}}> arguments, PipeTerminator pipeTerminator)
                                 => {{tupleOpenParenthesis}}{{arguments}}{{tupleClosingParenthesis}};
                         }
                         """);
    }
}
