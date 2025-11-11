namespace EmpInfoInfra.Models;

public partial class Location
{
    public int LocId { get; set; }

    public string? Location1 { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Country { get; set; }

    public virtual ICollection<WorkInfo> WorkInfos { get; set; } = new List<WorkInfo>();
}
