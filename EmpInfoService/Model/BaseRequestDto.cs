using Microsoft.AspNetCore.Http;

namespace EmpInfoService.Model
{
    public class BaseRequestDto
    {
        public string? EmpId { get; set; } 
        public IFormFile? Profile { get; set; } 

    }
}
