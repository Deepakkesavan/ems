

using EmpInfoService.Model;

namespace EmployeeInformationInner.Services.Interfaces
{
    public interface IEmployeeDetails
    {
        //public Task<List<EmployeeDto>> GetAllEmployees(string project);
        public List<EmployeeDto> GetAllEmployeesFromCache(string query);
        public List<EmployeeDto> GetEmployeeDetailsFromCache();

    }
}
