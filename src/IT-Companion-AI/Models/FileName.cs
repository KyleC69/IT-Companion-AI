using ApiMember = ITCompanionAI.EFModels.ApiMember;
using ApiParameter = ITCompanionAI.EFModels.ApiParameter;
using ApiType = ITCompanionAI.EFModels.ApiType;



namespace ITCompanionAI.Models;


/// <summary>
///     Represents a tree structure that encapsulates syntax-related information,
///     including types, members, and parameters extracted from source code.
/// </summary>
public sealed class SyntaxTypeTree
{
    public SyntaxTypeTree(ApiType typeSymbol, List<ApiMember> memberSymbols, List<ApiParameter> parameterSymbols)
    {
        ArgumentNullException.ThrowIfNull(typeSymbol);
        ArgumentNullException.ThrowIfNull(memberSymbols);
        ArgumentNullException.ThrowIfNull(parameterSymbols);

        TypeSymbol.Add(typeSymbol);
        MemberSymbols.AddRange(memberSymbols);
        ParameterSymbols.AddRange(parameterSymbols);
    }








    public List<ApiType> TypeSymbol { get; } = new();

    public List<ApiMember> MemberSymbols { get; } = new();

    public List<ApiParameter> ParameterSymbols { get; } = new();
}