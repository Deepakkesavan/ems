using System;
using System.Collections.Generic;

namespace EmpInfoInfra.Models;

public partial class PublicUser
{
    public int PublicId { get; set; }

    public string? PublicUsername { get; set; }

    public string? PublicEmail { get; set; }

    public string? PublicPassword { get; set; }

    public bool? IsFirstTimeLogin { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsActive { get; set; }
}
