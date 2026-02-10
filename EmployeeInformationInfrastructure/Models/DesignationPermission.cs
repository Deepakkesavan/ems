using System;
using System.Collections.Generic;

namespace EmpInfoInfra.Models;

public partial class DesignationPermission
{
    public Guid Id { get; set; }

    public Guid DesignationGuid { get; set; }

    public Guid PermissionGuid { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual Designation Designation { get; set; } = null!;
}
