using EmpInfoInner.Middleware;
using EmpInfoService.Model;
using EmpInfoService.Services.ServiceImpl;
using EmployeeInformationInner.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInformationInner.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeDetails _employeeDetails;
        private readonly EmpService _empService;

        public EmployeeController(IEmployeeDetails employeeDetails, EmpService empService)
        {
            _employeeDetails = employeeDetails;
            _empService = empService;
        }
        //retrives data from external custom cache library based on user query(EmployeeCacheManager)
        //[HttpGet("SharedCache")]
        //public Task<List<EmployeeDto>> GetAllEmployees(string project)
        //{
        //        return _employeeDetails.GetAllEmployees(project);
        //}


        private async Task<UserDto> GetUserDtoAsync()
        {
            if (HttpContext.User.Identity?.IsAuthenticated ?? false)
            {
                var empIdClaim = HttpContext.User.FindFirst("empId")?.Value;
                var designationClaim = HttpContext.User.FindFirst("designation")?.Value;

                if (int.TryParse(empIdClaim, out var empId))
                {
                    return new UserDto
                    {
                        EmpId = empId,
                        Designation = designationClaim ?? "Unknown",
                        Authenticated = true
                    };
                }
            }

            throw new Exception("Session expired or user not authenticated");
                    }





        //Retrives employees from in-memory cache based on user query
        [HttpGet("InMemoryCache")]
        public async Task<List<EmployeeDto>> GetAllEmployeesFromCache([FromQuery] string query)
        {
            var user = await GetUserDtoAsync();
            

            return _employeeDetails.GetAllEmployeesFromCache(query);
        }

        [HttpGet("TempCache")]
        public async Task<ActionResult<List<EmployeeDto>>> GetEmployeeDetailsFromCache()
        {
            var user = await GetUserDtoAsync();

            List<EmployeeDto> result= _employeeDetails.GetEmployeeDetailsFromCache();
            return Ok(result);
        }
        [HttpPost("GetAllEmployeeDetails")]
        public async Task<ActionResult<List<EmployeeDto>>> GetEmployeeDetails()
        {
            var user = await GetUserDtoAsync();

            List<EmployeeDto> result = await _empService.GetEmployeeDetails();
            return Ok(result);
        }
        [HttpPost("GetEmployeeDetailsById")]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeDetailsById()
        {
            UserDto user = await GetUserDtoAsync();
            Console.WriteLine(user.EmpId);
            //lo

            EmployeeDto result = await _empService.GetEmployeeDetailsById(user.EmpId.ToString());
            return Ok(result);
        }

        [HttpPost("UpdateEmployee")]
        public async Task<ActionResult<string>> UpdateEmployeeDetails([FromBody] UpdateEmployeeDto updatedEmployeeDto)
        {
            var user = await GetUserDtoAsync();

            Console.WriteLine(updatedEmployeeDto);
            string result = await _empService.UpdateEmployeeDetails(updatedEmployeeDto);
            return Ok(result);
        }
        [HttpPost("UploadPhoto")]
        public async Task<ActionResult<string>> UploadProfile([FromForm] BaseRequestDto requestDto)
        {
            var user = await GetUserDtoAsync();

            var result = await _empService.UploadProfile(requestDto);
            return result;
        }
        [HttpPost("Getprofile")]
        public async Task<IActionResult> GetProfile([FromBody] BaseRequestDto requestDto)
        {
            var user = await GetUserDtoAsync();

            var result = await _empService.GetProfile(requestDto);
            return File(result, "image/png");
        }

    }
}


