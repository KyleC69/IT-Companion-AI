using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;





namespace ITCompanionAI.AgentFramework.Ingestion;

public class ApiHarvester
{
    private string _startPath;
    private string _schemaPath = "D:\\Solutions\\SolHack\\RepoRoot\\src\\IT-Companion-AI\\JsonSchema\\IngestSchema.json";
    private CancellationTokenSource cts = new();
    private IngestionConfig config;









    public ApiHarvester(string startPath) 
    {
        _startPath = startPath = @"D:\SkApiRepo\semantic-kernel\dotnet\src";



    }

   public void StartHarvesting()
   {
        AiagentRagContext db = new();
        config = new IngestionConfig
        {
            IncludePrivate = false,
            DocPaths = ["docs/", "samples/"],
            ApiRoots = ["dotnet/src/", "dotnet/samples/"]
        };


        var rootPath = Path.Combine(_startPath);

        var csFiles = Directory
            .EnumerateFiles(rootPath, "*.cs", SearchOption.AllDirectories)
            .ToList();

        var syntaxTrees = csFiles
            .Select(path => CSharpSyntaxTree.ParseText(File.ReadAllText(path)))
            .ToList();

        var compilation = CSharpCompilation.Create(
            assemblyName: "SK",
            syntaxTrees: syntaxTrees,
            references: new[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
            }
        );

        var types = compilation.GlobalNamespace
            .GetNamespaceMembers()
            .SelectMany(CollectTypesRecursively)
            .ToList();

        foreach (var type in types)
        {
            var dto = new ApiType
            {
                Id = default,
                SourceSnapshotId = default,
                TypeUid = $"csharp:{type.ToDisplayString()}",
                Name = type.Name,
                Namespace = type.ContainingNamespace.ToDisplayString(),
                Kind = type.TypeKind.ToString()
                    .ToLowerInvariant(),
                Accessibility = type.DeclaredAccessibility.ToString()
                    .ToLowerInvariant(),
                IsStatic = type.IsStatic,
                IsGeneric = type.TypeParameters.Any(),
               // GenericParameters = type.TypeParameters.Select(p => p.Name),
                Summary = XmlDocExtractor.GetSummary(type),
                Remarks = XmlDocExtractor.GetRemarks(type),
                Attributes = AttributeExtractor.From(type),
                ApiMembers = null,
                SourceSnapshot = null,
                Members = MemberExtractor.From(type)
            };





            foreach (var m in type.GetMembers().OfType<IMethodSymbol>())
            {
            db.ApiMembers.Add(new ApiMember
            {
                Id = default,
                ApiTypeId = default,
                MemberUid = $"csharp:{m.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)}",
                Name = m.Name,
                Kind = "method",
                Accessibility = m.DeclaredAccessibility.ToString()
                    .ToLowerInvariant(),
                IsStatic = m.IsStatic,
                IsExtensionMethod = m.IsExtensionMethod,
                IsAsync = m.IsAsync,
                ReturnType = m.ReturnType.ToDisplayString(),
                Summary = XmlDocExtractor.GetSummary(m),
                Remarks = null,
                GenericParameters = null,
                Attributes = null,
                SourceFilePath = null,
                SourceStartLine = null,
                SourceEndLine = null,
                ApiMemberDocLinks = null,
                ApiParameters = null,
                ApiType = null,
                Parameters = ParameterExtractor.From(m),
                SourceLocation = SourceLocator.From(m),
                DocLinks = DocLinker.Resolve(m),

            });
            }

            foreach(var prop in type.GetMembers().OfType<IPropertySymbol>())
            {
              db.ApiMembers.Add(new ApiMember
              {
                  Id = default,
                  ApiTypeId = default,
                  MemberUid = $"csharp:{prop.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)}",
                  Name = prop.Name,
                  Kind = "property",
                  Accessibility = prop.DeclaredAccessibility.ToString()
                      .ToLowerInvariant(),
                  IsStatic = prop.IsStatic,
                  IsExtensionMethod = null,
                  IsAsync = null,
                  ReturnType = prop.Type.ToDisplayString(),
                  Summary = XmlDocExtractor.GetSummary(prop),
                  Remarks = null,
                  GenericParameters = null,
                  Attributes = null,
                  SourceFilePath = null,
                  SourceStartLine = null,
                  SourceEndLine = null,
                  ApiMemberDocLinks = null,
                  ApiParameters = null,
                  ApiType = null,
                  Parameters = null,
                  SourceLocation = SourceLocator.From(prop),
                  DocLinks = DocLinker.Resolve(prop)
              });
            }

            foreach(var eventSym in type.GetMembers().OfType<IEventSymbol>())
            {
                 db.ApiMembers.Add(new ApiMember
                 {
                     Id = default,
                     MemberUid = $"csharp:{eventSym.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)}",
                     Name = eventSym.Name,
                     Kind = "event",
                     Accessibility = eventSym.DeclaredAccessibility.ToString()
                         .ToLowerInvariant(),
                     IsStatic = eventSym.IsStatic,
                     IsExtensionMethod = null,
                     IsAsync = null,
                     ReturnType = null,
                     Summary = XmlDocExtractor.GetSummary(eventSym),
                     Remarks = null,
                     GenericParameters = null,
                     SourceLocation = SourceLocator.From(eventSym),
                     DocLinks = DocLinker.Resolve(eventSym),
                     ApiMemberDocLinks = null,
                     ApiParameters = null,
                     ApiType = null,
                     Parameters = null,
                     ApiTypeId = GetTypeId(eventSym),
                     Attributes = AttributeExtractor.From(eventSym),
                     SourceFilePath = null,
                     SourceStartLine = null,
                     SourceEndLine = null,

                 });
            }

            foreach(var field in type.GetMembers().OfType<IFieldSymbol>())
            {
                 db.ApiMembers.Add(new ApiMember
                {
                    MemberUid = $"csharp:{field.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)}",
                    Name = field.Name,
                    Kind = "field",
                    Accessibility = field.DeclaredAccessibility.ToString().ToLowerInvariant(),
                    IsStatic = field.IsStatic,
                    Summary = XmlDocExtractor.GetSummary(field),
                    SourceLocation = SourceLocator.From(field),
                    DocLinks = DocLinker.Resolve(field)
                });
            }





        }







        //________________________________________________
    }





   private Guid GetTypeId(IEventSymbol EventSym)
   {
       return default;
   }





   private static IEnumerable<INamedTypeSymbol> CollectTypesRecursively(INamespaceSymbol ns)
   {
       foreach (var nestedNs in ns.GetNamespaceMembers())
       {
           foreach (var type in CollectTypesRecursively(nestedNs))
               yield return type;
       }

       foreach (var type in ns.GetTypeMembers())
       {
           foreach (var t in CollectTypesRecursively(type))
               yield return t;
       }
   }

   private static IEnumerable<INamedTypeSymbol> CollectTypesRecursively(INamedTypeSymbol type)
   {
       yield return type;

       foreach (var nested in type.GetTypeMembers())
       {
           foreach (var t in CollectTypesRecursively(nested))
               yield return t;
       }
   }





    
}



public class DocLinker
{
    public static object Resolve(IFieldSymbol Field)
    {
        return null;
    }





    public static object Resolve(IPropertySymbol Field)
    {
        return null;
    }





    public static object Resolve(IEventSymbol EventSym)
    {
        return null;
    }





    public static object Resolve(IMethodSymbol EventSym)
    {
        return null;
    }
}



internal class IngestionConfig
{
    public bool IncludePrivate { get; set; }
    public string[]? DocPaths { get; set; }
    public string[]? ApiRoots { get; set; }
}

public interface IRoslynApiHarvester
{
    Task<ApiType> HarvestApiAsync(IngestionRequest request, CancellationToken ct);
}

public class RoslynApiHarvester 
{
    public async Task<object> HarvestApiAsync(IngestionRequest request, CancellationToken ct)
    {
        var csFiles = request.FilePath
            .SelectMany(root => Directory.EnumerateFiles(request.FilePath, "*.cs", SearchOption.AllDirectories))
            .ToList();

        var syntaxTrees = csFiles
            .Select(f => CSharpSyntaxTree.ParseText(File.ReadAllText(f)))
            .ToList();

        // var compilation = CSharpCompilation.Create("SK", syntaxTrees, MetadataReference.CreateFromFile("", MetadataReferenceProperties.Assembly));
        var compilation = CSharpCompilation.Create("SK", syntaxTrees);
        var types = RoslynTypeCollector.CollectTypes(compilation);

        var dtoTypes = types.Select(t => ApiTypeMapper.FromSymbol(t)).ToList();

        return new RawApiModel(dtoTypes);
    }
}



public class RawApiModel
{
    public RawApiModel(List<ApiType> DtoTypes)
    {
    }
}



public static class ApiTypeMapper
{
    public static ApiType FromSymbol(INamedTypeSymbol type)
    {
        return new ApiType
        {
            Id = default,
            SourceSnapshotId = default,
            TypeUid = $"csharp:{type.ToDisplayString()}",
            Name = type.Name,
            Namespace = type.ContainingNamespace.ToDisplayString(),
            Kind = type.TypeKind.ToString(),
            Accessibility = type.DeclaredAccessibility.ToString(),
            IsStatic = type.IsStatic,
            IsGeneric = type.TypeParameters.Any(),
            GenericParameters = null,
            Summary = XmlDocExtractor.GetSummary(type),
            Remarks = XmlDocExtractor.GetRemarks(type),
            Attributes = AttributeExtractor.From(type),
            ApiMembers = null,
            SourceSnapshot = null,
            Members = MemberExtractor.From(type)
        };
    }
}
public static class MemberExtractor
{
    public static List<ApiMember> From(INamedTypeSymbol type)
    {
        var list = new List<ApiMember>();

        foreach (var m in type.GetMembers().OfType<IMethodSymbol>())
        {
            list.Add(new ApiMember
            {
                Id = default,
                ApiTypeId = default,
                MemberUid = $"csharp:{m.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat)}",
                Name = m.Name,
                Kind = "method",
                Accessibility = m.DeclaredAccessibility.ToString(),
                IsStatic = m.IsStatic,
                IsExtensionMethod = null,
                IsAsync = m.IsAsync,
                ReturnType = m.ReturnType.ToDisplayString(),
                Summary = XmlDocExtractor.GetSummary(m),
                Remarks = null,
                GenericParameters = null,
                Attributes = null,
                SourceFilePath = null,
                SourceStartLine = null,
                SourceEndLine = null,
                ApiMemberDocLinks = null,
                ApiParameters = null,
                ApiType = null,
                Parameters = ParameterExtractor.From(m),
                SourceLocation = SourceLocator.From(m),
                DocLinks = null
            });
        }

        return list;
    }
}