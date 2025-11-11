using AutoMapper;
using EmpInfoInfra.Models;
using EmpInfoService.Model;
using Microsoft.AspNetCore.Mvc;

namespace EmpInfoService.Services.ServiceImpl
{
    public class AdminService
    {
        private readonly EmployeeDetailsContext _context;
        private readonly IMapper _mapper;
        public AdminService(EmployeeDetailsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        public async Task<ActionResult<string>> CreateEmployee(EmployeeDto employeeDto)
        {
            var employee = await _context.Employees.FindAsync(employeeDto.EmpId);
            if (employee != null)
            {
                throw new Exception($"Employee {employeeDto.FirstName + employeeDto.LastName} is already created");
            }
            employee = new Employee
            {
                EmpId = employeeDto.EmpId,
                Id = Guid.NewGuid(),
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                Email = employeeDto.Email,
                Profile = employeeDto.Profile,
                CreatedTime = DateTime.UtcNow,
                UpdatedTime = DateTime.UtcNow,
                CreatedBy = "Admin",
                WorkInfo = new WorkInfo
                {
                    SourceOfHire = employeeDto.SourceOfHire,
                    Doj = employeeDto.Doj,
                    Doc = employeeDto.Doc,
                    Status = employeeDto.Status,
                    CurrExp = employeeDto.CurrExp,
                    TotalExp = employeeDto.TotalExp,
                    TypeId = employeeDto.TypeId,
                    ReportingManager = employeeDto.ReportingManager,
                    EmailTriggerStatus = employeeDto.EmailTriggerStatus,
                    TransferFromDate = employeeDto.TransferFromDate,
                    CreatedTime = DateTime.UtcNow,
                    UpdatedTime = DateTime.UtcNow,
                    CreatedBy = "Admin"
                },
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return ($"Employee {employeeDto.FirstName + employeeDto.LastName} is created");


        }
        public async Task<ActionResult<string>> UpdateEmployee(EmployeeDto employeeDto)
        {
            var employee = await _context.Employees.FindAsync(employeeDto.EmpId);
            if (employee == null)
            {
                throw new Exception($"Employee {employeeDto.FirstName + employeeDto.LastName} is not found");
            }
            employee = new Employee
            {
                FirstName = employeeDto.FirstName,
                LastName = employeeDto.LastName,
                Email = employeeDto.Email,
                Profile = employeeDto.Profile,
                UpdatedTime = DateTime.UtcNow,
                UpdatedBy = "Admin",
                WorkInfo = new WorkInfo
                {
                    SourceOfHire = employeeDto.SourceOfHire,
                    Doj = employeeDto.Doj,
                    Doc = employeeDto.Doc,
                    Status = employeeDto.Status,
                    CurrExp = employeeDto.CurrExp,
                    TotalExp = employeeDto.TotalExp,
                    TypeId = employeeDto.TypeId,
                    ReportingManager = employeeDto.ReportingManager,
                    EmailTriggerStatus = employeeDto.EmailTriggerStatus,
                    TransferFromDate = employeeDto.TransferFromDate,
                    UpdatedTime = DateTime.UtcNow,
                    UpdatedBy = "Admin"
                },
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return ($"Updated Successfully");


        }
    }
}
