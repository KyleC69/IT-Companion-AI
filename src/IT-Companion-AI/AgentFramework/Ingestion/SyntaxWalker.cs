// Project Name: SKAgent
// File Name: SyntaxWalker.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz KyleC69
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


namespace ITCompanionAI.AgentFramework.Ingestion;


// Custom SyntaxWalker to collect method names and using directives
public class MySyntaxWalker : CSharpSyntaxWalker
{
    public MySyntaxWalker() : base(SyntaxWalkerDepth.StructuredTrivia)
    {
    }







    public List<string> MethodNames { get; } = new();
    public List<string> Usings { get; } = new();







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