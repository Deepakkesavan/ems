using EmpInfoInfra.Models;
using Microsoft.EntityFrameworkCore;

namespace EmpInfoService.Services.ServiceImpl
{
    public class BillingService
    {
        private readonly EmployeeDetailsContext _context;
        public BillingService(EmployeeDetailsContext context)
        {
            _context = context;
        }

        public async Task AddBilling(string status)
        {
            var billing = await _context.Billings
                .FirstOrDefaultAsync(x => x.Status == status);
            if (billing == null)
            {
                await _context.AddAsync(billing);
                await _context.SaveChangesAsync();
            }


        }
        public async Task DeleteBilling(Guid bId)
        {
            var billing = await _context.Billings
                .FirstOrDefaultAsync(x => x.Id == bId);
            if (billing == null)
            {
                //Exception
            }
            billing.IsActive = false;
            _context.Billings.Update(billing);
            await _context.SaveChangesAsync();

        }

        public async Task<Billing> GetBillingById(Guid Id)
        {
            var billing = await _context.Billings.FindAsync(Id);
            if (billing == null)
            {
                //NotFoundException
            }
            return billing;
        }

        public async Task<List<Billing>> GetAllBilling()
        {
            List<Billing> billing = await _context.Billings.ToListAsync();
            return billing;
        }
    }
}
