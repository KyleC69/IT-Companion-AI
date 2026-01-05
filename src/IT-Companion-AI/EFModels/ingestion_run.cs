using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ITCompanionAI.Context;

[Table("ingestion_run")]
public partial class ingestion_run
{
    [Key]
    public Guid id { get; set; }

    public DateTime timestamp_utc { get; set; }

    [StringLength(200)]
    public string schema_version { get; set; } = null!;

    public string? notes { get; set; }

    [InverseProperty("created_ingestion_run")]
    public virtual ICollection<api_feature> api_featurecreated_ingestion_runs { get; set; } = new List<api_feature>();

    [InverseProperty("removed_ingestion_run")]
    public virtual ICollection<api_feature> api_featureremoved_ingestion_runs { get; set; } = new List<api_feature>();

    [InverseProperty("updated_ingestion_run")]
    public virtual ICollection<api_feature> api_featureupdated_ingestion_runs { get; set; } = new List<api_feature>();

    [InverseProperty("created_ingestion_run")]
    public virtual ICollection<api_member> api_membercreated_ingestion_runs { get; set; } = new List<api_member>();

    [InverseProperty("removed_ingestion_run")]
    public virtual ICollection<api_member> api_memberremoved_ingestion_runs { get; set; } = new List<api_member>();

    [InverseProperty("updated_ingestion_run")]
    public virtual ICollection<api_member> api_memberupdated_ingestion_runs { get; set; } = new List<api_member>();

    [InverseProperty("created_ingestion_run")]
    public virtual ICollection<api_parameter> api_parametercreated_ingestion_runs { get; set; } = new List<api_parameter>();

    [InverseProperty("removed_ingestion_run")]
    public virtual ICollection<api_parameter> api_parameterremoved_ingestion_runs { get; set; } = new List<api_parameter>();

    [InverseProperty("updated_ingestion_run")]
    public virtual ICollection<api_parameter> api_parameterupdated_ingestion_runs { get; set; } = new List<api_parameter>();

    [InverseProperty("created_ingestion_run")]
    public virtual ICollection<api_type> api_typecreated_ingestion_runs { get; set; } = new List<api_type>();

    [InverseProperty("removed_ingestion_run")]
    public virtual ICollection<api_type> api_typeremoved_ingestion_runs { get; set; } = new List<api_type>();

    [InverseProperty("updated_ingestion_run")]
    public virtual ICollection<api_type> api_typeupdated_ingestion_runs { get; set; } = new List<api_type>();

    [InverseProperty("created_ingestion_run")]
    public virtual ICollection<code_block> code_blockcreated_ingestion_runs { get; set; } = new List<code_block>();

    [InverseProperty("removed_ingestion_run")]
    public virtual ICollection<code_block> code_blockremoved_ingestion_runs { get; set; } = new List<code_block>();

    [InverseProperty("updated_ingestion_run")]
    public virtual ICollection<code_block> code_blockupdated_ingestion_runs { get; set; } = new List<code_block>();

    [InverseProperty("created_ingestion_run")]
    public virtual ICollection<doc_page> doc_pagecreated_ingestion_runs { get; set; } = new List<doc_page>();

    [InverseProperty("removed_ingestion_run")]
    public virtual ICollection<doc_page> doc_pageremoved_ingestion_runs { get; set; } = new List<doc_page>();

    [InverseProperty("updated_ingestion_run")]
    public virtual ICollection<doc_page> doc_pageupdated_ingestion_runs { get; set; } = new List<doc_page>();

    [InverseProperty("created_ingestion_run")]
    public virtual ICollection<doc_section> doc_sectioncreated_ingestion_runs { get; set; } = new List<doc_section>();

    [InverseProperty("removed_ingestion_run")]
    public virtual ICollection<doc_section> doc_sectionremoved_ingestion_runs { get; set; } = new List<doc_section>();

    [InverseProperty("updated_ingestion_run")]
    public virtual ICollection<doc_section> doc_sectionupdated_ingestion_runs { get; set; } = new List<doc_section>();

    [InverseProperty("ingestion_run")]
    public virtual ICollection<source_snapshot> source_snapshots { get; set; } = new List<source_snapshot>();
}
