
using EmpInfoService.Model;
using EmpInfoService.Services.ServiceImpl;
using EmployeeInformationInner.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmpInfoInner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IEmployeeDetails _employeeDetails;
        private readonly AdminService _adminService;

        public AdminController(IEmployeeDetails employeeDetails, AdminService adminService)
        {
            _employeeDetails = employeeDetails;
            _adminService = adminService;
        }
        [HttpPost("CreateEmployee")]
        public async Task<ActionResult<string>> CreateEmployee([FromBody] EmployeeDto employeeDto)
        {
            ActionResult<string> result = await _adminService.CreateEmployee(employeeDto);
            return Ok(result);
        }
        [HttpPost("UpdateEmployee")]
        public async Task<ActionResult<string>> UpdateEmployee([FromBody] EmployeeDto employeeDto)
        {
            ActionResult<string> result = await _adminService.UpdateEmployee(employeeDto);
            return Ok(result);
        }
    }
}
