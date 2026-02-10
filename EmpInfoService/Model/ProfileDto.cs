using Microsoft.AspNetCore.Http;

namespace EmpInfoService.Model
{
    public class ProfileDto:BaseRequestDto
    {
        public IFormFile? Profile { get; set; }
    }
}
