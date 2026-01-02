using System;
using System.Collections.Generic;

namespace ITCompanionAI;

public partial class ApiParameter
{
    public Guid Id { get; set; }

    public Guid ApiMemberId { get; set; }

    public string? Name { get; set; }

    public string? Type { get; set; }

    public int? Position { get; set; }

    public bool? HasDefaultValue { get; set; }

    public string? DefaultValueLiteral { get; set; }

    public virtual ApiMember ApiMember { get; set; } = null!;
}
