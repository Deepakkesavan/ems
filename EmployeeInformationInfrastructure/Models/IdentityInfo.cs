namespace EmpInfoInfra.Models;

public partial class IdentityInfo
{
    public string? Uan { get; set; }

    public string? Pan { get; set; }

    public long? Aadhar { get; set; }

    public byte[]? AadharFile { get; set; }

    public byte[]? Panfile { get; set; }

    public string? Ifsc { get; set; }

    public string EmpId { get; set; } = null!;

    public DateTime CreatedTime { get; set; }

    public Guid Id { get; set; }

    public DateTime? UpdatedTime { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public bool IsActive { get; set; }

    public virtual Employee Emp { get; set; } = null!;
}
