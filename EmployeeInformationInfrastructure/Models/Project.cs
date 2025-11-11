namespace EmpInfoInfra.Models;

public partial class Project
{
    public int ProjId { get; set; }

    public string? ProjectName { get; set; }

    public DateTime CreatedTime { get; set; }

    public Guid Id { get; set; }

    public DateTime? UpdatedTime { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    public virtual ICollection<WorkInfo> WorkInfos { get; set; } = new List<WorkInfo>();
}
