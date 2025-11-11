using AutoMapper;
using EmpInfoInfra.Models;
using Microsoft.EntityFrameworkCore;

namespace EmpInfoService.Services.ServiceImpl
{
    public class ClientService
    {
        private readonly EmployeeDetailsContext _context;
        public ClientService(EmployeeDetailsContext context, IMapper mapper)
        {
            _context = context;       

        }
        public async Task AddClient(Client client)
        {
            var clients = await _context.Clients
                .FirstOrDefaultAsync(x => x.ClientName == client.ClientName);
            if (clients == null)
            {
                await _context.Clients.AddAsync(client);
                await _context.SaveChangesAsync();
            }

        }
        public async Task DeleteClient(int clientId)
        {
            var clients = await _context.Clients
                .FirstOrDefaultAsync(x => x.Cid == clientId);
            if (clients == null)
            {
                //Exception
            }
            clients.IsActive = false;
            _context.Clients.Update(clients);
            await _context.SaveChangesAsync();

        }

        public async Task<Client> GetClient(int clientId)
        {
            var clients = await _context.Clients.FindAsync(clientId);
            if (clients == null)
            {
                //NotFoundException
            }
            return clients;
        }

        public async Task<List<Client>> GetAllClients()
        {
            List<Client> clients = await _context.Clients.ToListAsync();
            return clients;
        }
    }
}
