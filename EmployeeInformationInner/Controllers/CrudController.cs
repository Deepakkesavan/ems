using EmpInfoInfra.Models;
using EmpInfoService.Services.ServiceImpl;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInformationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrudController : ControllerBase

    {
        private readonly DepartmentService _departmentService;
        private readonly DesignationService _designationService;
        public CrudController(DepartmentService departmentService, DesignationService designationService)
        {
            _departmentService = departmentService;
            _designationService = designationService;
        }
        [HttpPost("AddDepartment")]
        public async Task<string> AddDepartment([FromBody] string department)
        {
            return await _departmentService.AddDepartment(department);         
        }

        // Delete Department 
        [HttpDelete("DeleteDepartment")]
        public async Task<IActionResult> DeleteDepartment(Guid id)
        {
                var result = await _departmentService.DeleteDepartment(id);
                return Ok( result);
        }

        // Get All Departments
        [HttpGet("AllDepartments")]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments = await _departmentService.GetAllDepartments();
            return Ok(departments);
        }
       

        [HttpPost("AddDesignation")]
        public async Task<string> AddDesignation([FromBody] string designation)
        {
            return await _designationService.AddDesignation(designation);
        }

        // Delete Department 
        [HttpDelete("DeleteDesignation")]
        public async Task<IActionResult> DeleteDesignation(Guid id)
        {
            var result = await _departmentService.DeleteDepartment(id);
            return Ok(result);
        }
        // Get All Designations
        [HttpGet("AllDesignation")]
        public async Task<IActionResult> GetAllDesignations()
        {
            var departments = await _designationService.GetAllDesignations();
            return Ok(departments);
        }


    }
}
