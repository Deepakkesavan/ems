using System;
using System.Collections.Generic;

namespace EmpInfoInfra.CatalogModels;

public partial class Skill
{
    public Guid Id { get; set; }

    public string SkillName { get; set; } = null!;

    public string? CreatedBy { get; set; }

    public DateTimeOffset CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTimeOffset UpdatedDate { get; set; }

    public bool IsActive { get; set; }
}
