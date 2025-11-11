using AutoMapper;
using EmpInfoInfra.Models;
using Microsoft.EntityFrameworkCore;

namespace EmpInfoService.Services.ServiceImpl
{
    public class DesignationService
    {
        private readonly EmployeeDetailsContext _context;
        public DesignationService(EmployeeDetailsContext context, IMapper mapper)
        {
            _context = context;

        }
        public async Task<string> AddDesignation(string desg)
        {
            var designation = await _context.Designations
                .FirstOrDefaultAsync(x => x.Desg == desg);
            if (designation != null)
            {
                throw new Exception("Designation already exists");
            }
            await _context.Designations.AddAsync(designation);
            await _context.SaveChangesAsync();
            return "Created Successfully";

        }
        public async Task DeleteDesignation(Guid desgId)
        {
            var designation = await _context.Designations
                .FirstOrDefaultAsync(x => x.Id == desgId);
            if (designation == null)
            {
                throw new Exception("Designation Not Found");
            }
            designation.IsActive = false;
            _context.Designations.Update(designation);
            await _context.SaveChangesAsync();

        }

        public async Task<Designation> GetDesignationById(Guid Id)
        {
            var designation = await _context.Designations.FindAsync(Id);
            if (designation == null)
            {
                throw new Exception("Designation Not Found");
            }
            return designation;
        }

        public async Task<List<Designation>> GetAllDesignations()
        {
            List<Designation> designations = await _context.Designations.ToListAsync();
            return designations;
        }
    }
}
