using System;
using System.Collections.Generic;
using System.Text;

namespace ITCompanionAI.AgentFramework.Models;

public sealed class ApiTypeExtraction
{
    public string SemanticUid { get; set; }
    public string Name { get; set; }
    public string Kind { get; set; } // class, struct, interface, enum
    public string Accessibility { get; set; }
    public List<string> Modifiers { get; set; }
    public string BaseType { get; set; }
    public List<string> Interfaces { get; set; }
    public List<string> GenericParameters { get; set; }
    public List<string> GenericConstraints { get; set; }
    public List<string> Attributes { get; set; }
    public string Summary { get; set; }
    public string Remarks { get; set; }
    public string SourceFilePath { get; set; }
    public int StartLine { get; set; }
    public int EndLine { get; set; }
}


public sealed class ApiMemberExtraction
{
    public string SemanticUid { get; set; }
    public string Name { get; set; }
    public string Kind { get; set; } // Method, Property, Field, Event
    public string Accessibility { get; set; }
    public List<string> Modifiers { get; set; }
    public string ReturnType { get; set; } // methods/properties
    public List<ApiParameterExtraction> Parameters { get; set; }
    public List<string> GenericParameters { get; set; }
    public List<string> GenericConstraints { get; set; }
    public List<string> Attributes { get; set; }
    public string Summary { get; set; }
    public string Remarks { get; set; }
    public string SourceFilePath { get; set; }
    public int StartLine { get; set; }
    public int EndLine { get; set; }
}


public sealed class ApiParameterExtraction
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string Nullable { get; set; }
    public string Modifier { get; set; } // ref, out, in, params
    public string DefaultValue { get; set; }
    public List<string> Attributes { get; set; }
}

