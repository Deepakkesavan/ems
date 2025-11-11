using AutoMapper;
using EmpInfoInfra.Models;
using Microsoft.EntityFrameworkCore;

namespace EmpInfoService.Services.ServiceImpl
{
    public class ProjectService
    {
        private readonly EmployeeDetailsContext _context;
        public ProjectService(EmployeeDetailsContext context, IMapper mapper)
        {
            _context = context;

        }
        public async Task AddProject(string project)
        {
            var projects = await _context.Projects
                .FirstOrDefaultAsync(x => x.ProjectName == project);
            if (projects == null)
            {
                await _context.Projects.AddAsync(projects);
                await _context.SaveChangesAsync();
            }

        }
        public async Task DeleteProject(Guid projId)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(x => x.Id == projId);
            if (project == null)
            {
                throw new Exception("Designation Not Found");
            }
            project.IsActive = false;
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();

        }

        public async Task<Project> GetProjectById(Guid Id)
        {
            var project = await _context.Projects.FindAsync(Id);
            if (project == null)
            {
                throw new Exception("Designation Not Found");
            }
            return project;
        }

        public async Task<List<Project>> GetAllProjects()
        {
            List<Project> projects = await _context.Projects.ToListAsync();
            return projects;
        }
    }
}
