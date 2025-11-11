using EmpInfoInfra.Models;
using Microsoft.EntityFrameworkCore;

namespace EmpInfoService.Services.ServiceImpl
{
    public class EmployeeTypeService
    {
        private readonly EmployeeDetailsContext _context;
        public EmployeeTypeService(EmployeeDetailsContext context)
        {
            _context = context;
        }

        public async Task AddEmpType(string type)
        {
            var empType = await _context.EmpTypes
                .FirstOrDefaultAsync(x => x.Type == type);
            if (empType == null)
            {
                await _context.EmpTypes.AddAsync(empType);
                await _context.SaveChangesAsync();
            }


        }
        public async Task DeleteEmpType(Guid typeId)
        {
            var empType = await _context.EmpTypes
                .FirstOrDefaultAsync(x => x.Id == typeId);
            if (empType == null)
            {
                //Exception
            }
            empType.IsActive = false;
            _context.EmpTypes.Update(empType);
            await _context.SaveChangesAsync();

        }

        public async Task<EmpType> GetEmpTypeById(Guid Id)
        {
            var empType = await _context.EmpTypes.FindAsync(Id);
            if (empType == null)
            {
                throw new Exception("Not found");
            }
            return empType;
        }

        public async Task<List<EmpType>> GetAllEmpTypes()
        {
            List<EmpType> empType = await _context.EmpTypes.ToListAsync();
            return empType;
        }
    }
}
