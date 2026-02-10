using System;
using System.Collections.Generic;

namespace EmpInfoInfra.Models;

public partial class Client
{
    public int Cid { get; set; }

    public string? ClientName { get; set; }

    public int? ProjId { get; set; }

    public DateTime CreatedTime { get; set; }

    public Guid Id { get; set; }

    public DateTime? UpdatedTime { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public bool IsActive { get; set; }

    public virtual Project? Proj { get; set; }
}
