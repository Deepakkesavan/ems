using System;
using System.Collections.Generic;

namespace EmpInfoInfra.Models;

public partial class PersonalDetail
{
    public DateTime? Dob { get; set; }

    public string? Age { get; set; }

    public string? Gender { get; set; }

    public string? EmergencyContact1 { get; set; }

    public string? PersonalPhoneNumber { get; set; }

    public string? EmergencyContactName1 { get; set; }

    public string? EmergencyContactName2 { get; set; }

    public string? EmergencyContact2 { get; set; }

    public string? PermanentAddress { get; set; }

    public string? PresentAddress { get; set; }

    public string EmpId { get; set; } = null!;

    public DateTime CreatedTime { get; set; }

    public Guid Id { get; set; }

    //public DateTime? UpdatedTime { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public bool IsActive { get; set; }

    public string? PersonalEmail { get; set; }

    public virtual Employee Emp { get; set; } = null!;
}
