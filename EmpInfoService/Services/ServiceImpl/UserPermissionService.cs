using EmpInfoInfra.CatalogModels;
using EmpInfoInfra.Models;
using EmpInfoService.Common;
using EmpInfoService.Model;
using GlobalExceptionHandler.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace EmpInfoService.Services.ServiceImpl
{
    public class UserPermissionService
    {
        private readonly EmployeeDetailsContext _context;
        private readonly CatalogContext _catalogContext;
        private readonly CommonService _commonService;

        public UserPermissionService(EmployeeDetailsContext context,CatalogContext catalogContext,CommonService commonService)
        {
            _context = context;
            _catalogContext = catalogContext;
            _commonService = commonService;
        }

        public async Task<Dictionary<string, bool>> GetUserAccess(string? empId)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.EmpId == empId);
            if ( employee==null)
            {
                throw new DataNotFoundException($"Employee: {empId} not found");
            }
            var permissionsList = await _commonService.GetPermissionAsync();
            var employeeDesignation= await _context.Employees.Where(x => x.EmpId == empId)
                .AsNoTracking()
                .Select(x => x.WorkInfo.DesgnId)
                .FirstOrDefaultAsync();

            var designationPermission = await _context.DesignationPermissions
                .AsNoTracking()
                .Where(x => x.DesignationGuid == employeeDesignation)
                .Select(x => x.PermissionGuid)
                .ToListAsync();
            var permissions = permissionsList
                    .Where(p => designationPermission.Contains(p.Id))
                    .Select(p => p.AppGuid)
                    .Distinct()
                    .ToList();

            var applications =await _catalogContext.Applications.ToListAsync();
            var result = new Dictionary<string, bool>();

            foreach (var app in applications)
            {
                bool hasAnyPermission = permissions.Contains(app.Id);
                result[app.Name.ToLower()] = hasAnyPermission;
            }
            return result;
        }
        public async Task<Dictionary<string, bool>> CheckUserAppAccess(BaseRequestDto requestDto)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.EmpId == requestDto.EmpId);
            if (employee == null)
            {
                throw new DataNotFoundException($"Employee: {requestDto.EmpId} not found");
            }
            var employeeDesignation = await _context.Employees.Where(x => x.EmpId == requestDto.EmpId)
                .AsNoTracking()
                .Select(x => x.WorkInfo.DesgnId)
                .FirstOrDefaultAsync();

            var designationPermission = await _context.DesignationPermissions
                    .AsNoTracking()
                .Where(x => x.DesignationGuid == employeeDesignation)
                .Select(x => x.PermissionGuid)
                .ToListAsync();

            var permissions = await _catalogContext.Permissions
                    .AsNoTracking()
                    .Where(p => p.PermissionName.ToLower()=="approve")
                    .Select(p => p.AppGuid)
                    .Distinct()
                    .ToListAsync();

            var applications = await _catalogContext.Applications.FirstOrDefaultAsync(x=>x.Name== requestDto.Application);
            var result = new Dictionary<string, bool>();
                bool hasAnyPermission = permissions.Contains(applications.Id);
                result["approve"] = hasAnyPermission;
            return result;
        }
    }
}
