using System;
using System.Collections.Generic;

namespace EmpInfoInfra.Models;

public partial class WorkInfo
{
    public int? LocId { get; set; }

    public int? DeptId { get; set; }

    public string? SourceOfHire { get; set; }

    public DateTime? Doj { get; set; }

    public DateTime? Doc { get; set; }

    public string? Status { get; set; }

    public string? CurrExp { get; set; }

    public string? TotalExp { get; set; }

    public int? RoleId { get; set; }

    public int? Bid { get; set; }

    public string? EmailTriggerStatus { get; set; }

    public DateTime? TransferFromDate { get; set; }

    public int? TypeId { get; set; }

    public Guid? DesgnId { get; set; }

    public string? ReportingManager { get; set; }

    public string EmpId { get; set; } = null!;

    public int? ProjId { get; set; }

    public DateTime CreatedTime { get; set; }

    public Guid Id { get; set; }

    //public DateTime? UpdatedTime { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public bool IsActive { get; set; }

    public string? ManagerEmpCode { get; set; }

    public virtual Billing? BidNavigation { get; set; }

    public virtual Department? Dept { get; set; }

    public virtual Designation? Desgn { get; set; }

    public virtual Employee Emp { get; set; } = null!;

    public virtual Location? Loc { get; set; }

    public virtual Project? Proj { get; set; }

    public virtual Role? Role { get; set; }

    public virtual EmpType? Type { get; set; }
}
