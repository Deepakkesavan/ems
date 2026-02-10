using System.Text.Json.Serialization;

namespace EmpInfoService.Model
{
    public class OrgChartDto
    {
        public string EmpId { get; set; }
        public string? Name { get; set; }
        public string? Designation { get; set; }
        public string? Department { get; set; }
        public string? Email { get; set; } 
        public string? Phone { get; set; }
        //public byte[]? Profile { get; set; }
        public string? Location { get; set; }
        public string? ReportsTo { get; set; }
        public string? HireDate { get; set; }
        public int ? ReportsCount { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<OrgChartDto>? Children { get; set; } = new();


    }
}
