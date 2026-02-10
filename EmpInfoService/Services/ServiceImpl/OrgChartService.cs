using AutoMapper;
using EmpInfoInfra.Models;
using EmpInfoService.Model;
using GlobalExceptionHandler.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace EmpInfoService.Services.ServiceImpl
{
    public class OrgChartService
    {
        private readonly EmployeeDetailsContext _context;
        private readonly IMapper _mapper;

        public OrgChartService(EmployeeDetailsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        public async Task<UserDto> GetCurrentUser(string empId)
        {
           var employee = await _context.Employees
                .Include(i => i.PersonalDetail)
                .Include(j => j.WorkInfo)
                    .ThenInclude(w => w.Desgn)
                .FirstOrDefaultAsync(e => e.EmpId == empId);
            if (employee == null)
            {
                throw new DataNotFoundException("Employee not found");
            }
             return new UserDto
            {
                EmpId = employee.EmpId,
                Designation = employee.WorkInfo?.Desgn?.Desg,

            };
        }

        public async Task<List<OrgChartDto>> GetOrgChart()
        {
            var orgChartData = await _context.Employees
                   .Include(i => i.PersonalDetail)
                   .Include(j => j.WorkInfo)
                       .ThenInclude(w => w.Desgn)
                       .Include(j => j.WorkInfo)
                       .ThenInclude(w => w.Dept)
                   .Include(j => j.WorkInfo)
                       .ThenInclude(w => w.Role)
                   .Include(j => j.WorkInfo)
                       .ThenInclude(w => w.Proj)
                   .Include(k => k.IdentityInfo)
                   .ToListAsync();
            List<OrgChartDto> orgCharts = new();
            orgCharts = _mapper.Map<List<OrgChartDto>>(orgChartData);
            // Build a lookup dictionary to make the search easier
            var employeeDict = orgCharts
            .Where(e => !string.IsNullOrEmpty(e.EmpId))
            .ToDictionary(e => e.EmpId);

            // Build parent-child relationships
            foreach (var employee in orgCharts)
            {
                //This checks whether the current employee actually reports to someone and also checks for the who he reports to and stores in manager
                if (!string.IsNullOrEmpty(employee.ReportsTo) && employeeDict.TryGetValue(employee.ReportsTo, out var reportsTo))
                {
                    reportsTo?.Children?.Add(employee);

                }

            }
            foreach (var employee in orgCharts)
            {
                employee.ReportsCount = CountAllChildren(employee);
            }

            return orgCharts;
        }
        public async Task<OrgChartDto> GetOrgChartMyTeam(string EmpId)
        {
            var orgChartData = await _context.Employees
                   .Include(i => i.PersonalDetail)
                   .Include(j => j.WorkInfo)
                       .ThenInclude(w => w.Desgn)
                   .Include(j => j.WorkInfo)
                       .ThenInclude(w => w.Dept)
                   .Include(j => j.WorkInfo)
                       .ThenInclude(w => w.Role)
                   .Include(j => j.WorkInfo)
                       .ThenInclude(w => w.Proj)
                   .Include(k => k.IdentityInfo)
                   .ToListAsync();
            List<OrgChartDto> orgCharts = new();
            orgCharts = _mapper.Map<List<OrgChartDto>>(orgChartData);
            // Build a lookup dictionary to make the search easier
            var employeeDict = orgCharts
            .Where(e => !string.IsNullOrEmpty(e.EmpId))
            .ToDictionary(e => e.EmpId);

            // Current Employee
            if (string.IsNullOrEmpty(EmpId) || !employeeDict.TryGetValue(EmpId, out var currentEmployee))
                throw new DataNotFoundException("Employee not found");

            if (string.IsNullOrEmpty(currentEmployee.ReportsTo) || !employeeDict.TryGetValue(currentEmployee.ReportsTo, out var manager))
                return null;

            List<OrgChartDto> siblings = orgCharts.Where(e => e.ReportsTo == currentEmployee.ReportsTo).ToList();

            currentEmployee.Children = orgCharts
                .Where(e => e.ReportsTo == currentEmployee.EmpId)
                .ToList();

            manager.Children = siblings;

            return manager;
        }

        private int CountAllChildren(OrgChartDto employee)
        {
            if (employee.Children == null || employee.Children.Count == 0)
            {
                return 0;
            }
            Console.WriteLine(employee.Name);
            int count = employee.Children.Count;
            foreach (var child in employee.Children)
            {
                count += CountAllChildren(child);
            }
            return count;
        }
    }
}

