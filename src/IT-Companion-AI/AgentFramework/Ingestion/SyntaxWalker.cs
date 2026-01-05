using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace ITCompanionAI.AgentFramework.Ingestion;

// Custom SyntaxWalker to collect method names and using directives
public class MySyntaxWalker : CSharpSyntaxWalker
{
    public List<string> MethodNames { get; } = new();
    public List<string> Usings { get; } = new();

    public MySyntaxWalker() : base(SyntaxWalkerDepth.StructuredTrivia) { }

    public override void VisitUsingDirective(UsingDirectiveSyntax node)
    {
        Usings.Add(node.Name.ToString());
        base.VisitUsingDirective(node);
    }

    public override void VisitMethodDeclaration(MethodDeclarationSyntax node)
    {
        MethodNames.Add(node.Identifier.Text);
        base.VisitMethodDeclaration(node);
    }



   public static void WalkerMain(Solution solution)
    {
       

        try
        {


            // Walk the syntax tree
           
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

