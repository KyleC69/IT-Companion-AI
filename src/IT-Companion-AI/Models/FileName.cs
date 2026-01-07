// Project Name: SKAgent
// File Name: FileName.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using ITCompanionAI.Entities;


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