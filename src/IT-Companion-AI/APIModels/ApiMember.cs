using System;
using System.Collections.Generic;

using ITCompanionAI.AgentFramework.Ingestion;


namespace ITCompanionAI;

public partial class ApiMember
{
    public Guid Id { get; set; }

    public Guid ApiTypeId { get; set; }

    public string MemberUid { get; set; } = null!;

    public string? Name { get; set; }

    public string? Kind { get; set; }

    public string? Accessibility { get; set; }

    public bool? IsStatic { get; set; }

    public bool? IsExtensionMethod { get; set; }

    public bool? IsAsync { get; set; }

    public string? ReturnType { get; set; }

    public string? Summary { get; set; }

    public string? Remarks { get; set; }

    public string? GenericParameters { get; set; }

    public string? Attributes { get; set; }

    public string? SourceFilePath { get; set; }

    public int? SourceStartLine { get; set; }

    public int? SourceEndLine { get; set; }

    public virtual ICollection<ApiMemberDocLink> ApiMemberDocLinks { get; set; } = new List<ApiMemberDocLink>();

    public virtual ICollection<ApiParameter> ApiParameters { get; set; } = new List<ApiParameter>();

    public virtual ApiType ApiType { get; set; } = null!;
    public List<ApiParameter> Parameters { get; internal set; }
    public ApiSourceLocation? SourceLocation { get; internal set; }
    public object DocLinks { get; internal set; }
}
