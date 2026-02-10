using System;
using System.Collections.Generic;

namespace EmpInfoInfra.CatalogModels;

public partial class Permission
{
    public Guid Id { get; set; }

    public Guid AppGuid { get; set; }

    public string PermissionName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual Application App { get; set; } = null!;
}
