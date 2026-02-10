using System;
using System.Collections.Generic;

namespace EmpInfoInfra.CatalogModels;

public partial class Application
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}
