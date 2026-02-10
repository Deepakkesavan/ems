using System;
using System.Collections.Generic;

namespace EmpInfoInfra.CatalogModels;

public partial class Catalog
{
    public Guid Id { get; set; }

    public string Type { get; set; } = null!;

    public string Key { get; set; } = null!;

    public string Value { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public bool IsActive { get; set; }
}
