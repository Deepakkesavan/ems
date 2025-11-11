using EmpInfoInfra.Models;
using Microsoft.EntityFrameworkCore;

namespace EmpInfoService.Services.ServiceImpl
{
    public class RoleService
    {
        private readonly EmployeeDetailsContext _context;
        public RoleService(EmployeeDetailsContext context)
        {
            _context = context;
        }

        public async Task AddRole(Role role)
        {
            var roles = await _context.Roles
                .FirstOrDefaultAsync(x => x.RoleName == role.RoleName);
            if (roles != null)
            {
                throw new Exception($"the role {role.RoleName}is already there");
            }
            await _context.Roles.AddAsync(roles);
            await _context.SaveChangesAsync();


        }
        public async Task DeleteRole(Guid roleId)
        {
            var role = await _context.Roles
                .FirstOrDefaultAsync(x => x.Id == roleId);
            if (role == null)
            {
                throw new Exception("Role Not Found");
            }
            role.IsActive = false;
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();

        }

        public async Task<Role> GetRoleById(Guid Id)
        {
            var role = await _context.Roles.FindAsync(Id);
            if (role == null)
            {
                throw new Exception("Role Not Found");
            }
            return role;
        }

        public async Task<List<Role>> GetAllRoles()
        {
            List<Role> roles = await _context.Roles.ToListAsync();
            return roles;
        }
    }
}
