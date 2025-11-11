using AutoMapper;
using EmpInfoInfra.Models;
using Microsoft.EntityFrameworkCore;

namespace EmpInfoService.Services.ServiceImpl
{
    public class DepartmentService
    {
            private readonly EmployeeDetailsContext _context;
            public DepartmentService(EmployeeDetailsContext context, IMapper mapper)
            {
                _context = context;

            }
            public async Task<string> AddDepartment(string dept)
            {
            var department = await _context.Departments
                .FirstOrDefaultAsync(x => x.Department1 ==dept);
            if (department != null)
            {
                throw new Exception("Department already exists");
            }
            await _context.AddAsync(department);
            await _context.SaveChangesAsync();
            return "Department Created";

        }
            public async Task<string> DeleteDepartment(Guid deptId)
            {
                var department = await _context.Departments
                    .FirstOrDefaultAsync(x => x.Id == deptId);
                if (department == null)
                {
                throw new Exception("Department doesn't exists");
                }
                department.IsActive = false;
                _context.Departments.Update(department);
                await _context.SaveChangesAsync();
                return "Deleted Successfully";

            }

            //public async Task<Department> GetDepartmentById(Guid Id)
            //{
            //    var department = await _context.Departments.FindAsync(Id);
            //    if (department == null)
            //    {
            //    throw new Exception("Department doesn't exists");
            //    }
            //return department;
            //}

            public async Task<List<Department>> GetAllDepartments()
            {
                List<Department> departments = await _context.Departments.ToListAsync();
                return departments;
            }
        }
}
