namespace EmpInfoInfra.Models;

public partial class Employee
{
    public string EmpId { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string Email { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? LastModified { get; set; }

    public byte[]? Profile { get; set; }

    public DateTime CreatedTime { get; set; }

    public DateTime? UpdatedTime { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public bool IsActive { get; set; }

    public Guid Id { get; set; }

    public virtual IdentityInfo? IdentityInfo { get; set; }

    public virtual PersonalDetail? PersonalDetail { get; set; }

    public virtual WorkInfo? WorkInfo { get; set; }
}
