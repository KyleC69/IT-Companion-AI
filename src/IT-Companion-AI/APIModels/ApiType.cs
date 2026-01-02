using System;
using System.Collections.Generic;

namespace ITCompanionAI;

public partial class ApiType
{
    public Guid Id { get; set; }

    public Guid SourceSnapshotId { get; set; }

    public string TypeUid { get; set; } = null!;

    public string? Name { get; set; }

    public string? Namespace { get; set; }

    public string? Kind { get; set; }

    public string? Accessibility { get; set; }

    public bool? IsStatic { get; set; }

    public bool? IsGeneric { get; set; }

    public string? GenericParameters { get; set; }

    public string? Summary { get; set; }

    public string? Remarks { get; set; }

    public string? Attributes { get; set; }

    public virtual ICollection<ApiMember> ApiMembers { get; set; } = new List<ApiMember>();

    public virtual SourceSnapshot? SourceSnapshot { get; set; } = null!;
    public List<ApiMember> Members { get; internal set; }
}
