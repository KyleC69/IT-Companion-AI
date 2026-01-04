// Project Name: SKAgent
// File Name: execution_result.cs
// Author: Kyle Crowder
// Github:  OldSkoolzRoolz
// License: All Rights Reserved. No use without consent.
// Do not remove file headers


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;


namespace ITCompanionAI.AIVectorDb;


[Table("execution_result")]
[Index("execution_run_id", Name = "idx_execution_result_run_id")]
[Index("sample_uid", Name = "idx_execution_result_sample_uid")]
public class execution_result
{
    [Key] public Guid id { get; set; }

    public Guid execution_run_id { get; set; }

    [StringLength(200)] public string sample_uid { get; set; } = null!;

    [StringLength(100)] public string? status { get; set; }

    public string? build_log { get; set; }

    public string? run_log { get; set; }

    public string? exception_json { get; set; }

    public int? duration_ms { get; set; }

    [ForeignKey("execution_run_id")]
    [InverseProperty("execution_results")]
    public virtual execution_run execution_run { get; set; } = null!;
}