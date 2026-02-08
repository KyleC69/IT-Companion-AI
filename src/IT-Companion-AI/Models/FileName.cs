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








    public List<ApiType> TypeSymbol { get; } = [];

    public List<ApiMember> MemberSymbols { get; } = [];

    public List<ApiParameter> ParameterSymbols { get; } = [];
}