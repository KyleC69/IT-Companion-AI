using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ITCompanionAI.Context;

[Table("execution_result")]
[Index("sample_uid", "status", Name = "ix_execution_result_sample_status")]
public partial class execution_result
{
    [Key]
    public Guid id { get; set; }

    public Guid execution_run_id { get; set; }

    [StringLength(1000)]
    public string sample_uid { get; set; } = null!;

    [StringLength(100)]
    public string? status { get; set; }

    public string? build_log { get; set; }

    public string? run_log { get; set; }

    public string? exception_json { get; set; }

    public int? duration_ms { get; set; }

    [ForeignKey("execution_run_id")]
    [InverseProperty("execution_results")]
    public virtual execution_run execution_run { get; set; } = null!;
}
