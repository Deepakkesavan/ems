using EmpInfoInner.Middleware;
using EmpInfoService.Common;
using EmpInfoService.Model;
using EmpInfoService.Services.ServiceImpl;
using EmployeeInformationInner.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInformationInner.Controllers
{
    [Route("api/[controller]")]
    public class EmployeeController :ControllerBase {
        private readonly IEmployeeDetails _employeeDetails;
        private readonly EmpService _empService;
        private readonly UserPermissionService _userPermissionService;
        private readonly CommonService _commonService;

        public EmployeeController(IEmployeeDetails employeeDetails,EmpService empService,UserPermissionService userPermissionService,CommonService commonService) {
            _employeeDetails = employeeDetails;
            _empService = empService;
            _userPermissionService = userPermissionService;
            _commonService = commonService;
        }
        //retrives data from external custom cache library based on user query(EmployeeCacheManager)
        //[HttpGet("SharedCache")]
        //public Task<List<EmployeeDto>> GetAllEmployees(string project)
        //{
        //        return _employeeDetails.GetAllEmployees(project);
        //}




        //Retrives employees from in-memory cache based on user query
        [HttpGet("InMemoryCache")]
        public async Task<List<EmployeeDto>> GetAllEmployeesFromCache([FromQuery] string query) {
            var user = await _commonService.GetUserDtoAsync();


            return _employeeDetails.GetAllEmployeesFromCache(query);
        }

        [HttpGet("TempCache")]
        public async Task<ActionResult<List<EmployeeDto>>> GetEmployeeDetailsFromCache() {
            var user = await _commonService.GetUserDtoAsync();

            List<EmployeeDto> result = _employeeDetails.GetEmployeeDetailsFromCache();
            return Ok(result);
        }
        [HttpPost("GetAllEmployee")]
        public async Task<ActionResult<List<EmployeeDto>>> GetEmployeeDetails() {
            var user = await _commonService.GetUserDtoAsync();

            List<EmployeeDto> result = await _empService.GetEmployeeDetails();
            return Ok(result);
        }
        [HttpPost("GetEmployeeById")]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeDetailsById() {
            UserDto user = await _commonService.GetUserDtoAsync();
            Console.WriteLine(user.EmpId);
            EmployeeDto result = await _empService.GetEmployeeDetailsById(user.EmpId.ToString());
            return Ok(result);
        }

        [HttpPost("UpdateEmployee")]
        public async Task<ActionResult<string>> UpdateEmployeeDetails([FromBody] UpdateEmployeeDto updatedEmployeeDto) {
            var user = await _commonService.GetUserDtoAsync();

            Console.WriteLine(updatedEmployeeDto);
            string result = await _empService.UpdateEmployeeDetails(updatedEmployeeDto);
            return Ok(result);
        }
        [HttpPost("UploadProfile")]
        public async Task<ActionResult<string>> UploadProfile([FromForm] ProfileDto profileDto) {
            var user = await _commonService.GetUserDtoAsync();

            var result = await _empService.UploadProfile(profileDto);
            return result;
        }
        [HttpPost("GetProfile")]
        public async Task<IActionResult> GetProfile([FromBody] BaseRequestDto requestDto) {
            var user = await _commonService.GetUserDtoAsync();

            var result = await _empService.GetProfile(requestDto);
            return File(result,"image/png");
        }

        [HttpPost("CheckUserAccess")]
        public async Task<Dictionary<string,bool>> GetUserAccess([FromBody] BaseRequestDto requestDto) {
            var user = await _commonService.GetUserDtoAsync();

            return await _userPermissionService.GetUserAccess(user.EmpId.ToString());
        }

        [HttpPost("CheckUserAppAccess")]
        public async Task<Dictionary<string,bool>> CheckUserAppAccess([FromBody] BaseRequestDto requestDto) {
            var user = await _commonService.GetUserDtoAsync();

            return await _userPermissionService.CheckUserAppAccess(requestDto);
        }


        [HttpPost("Basic")]
        public async Task<List<EmployeeBasicDto>> GetEmployeeBasicDetails() {
            return await _empService.GetEmployeeBasicDetails();
        }


        [HttpPost("Email/Exist")]
        public async Task<bool> DoesEmployeeEmailExist([FromBody] EmployeeEmailDto req) {
           
            return await _empService.DoesEmpEmailExist(req);
        }

    }
}


