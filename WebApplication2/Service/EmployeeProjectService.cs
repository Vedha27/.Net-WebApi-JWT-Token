using Microsoft.EntityFrameworkCore;
using WebApplication2.DTO;
using WebApplication2.MDbContext;
using WebApplication2.Models;
using WebApplication2.Repositories;

namespace WebApplication2.Service
{
    public class EmployeeProjectService: IEmployeeProjectRepository
    {
        private readonly MyDbContext _dbContext;
        public EmployeeProjectService(MyDbContext ctxt)
        {
            _dbContext=ctxt;
        }


        public async Task<List<Project>> GetProjectsByEmployeeIdAsync(int employeeId)
        {
            return await _dbContext.Employees
                .Where(e => e.Id == employeeId)
                .Include(p=> p.Projects)
                .SelectMany(p=>p.Projects)

                .ToListAsync();
        }       
        public async Task<List<Employee>> GetEmployeesByProjectIdAsync(int projectId)
        {
            return await _dbContext.Projects
                .Where(p => p.ProjectId == projectId)
                .Include(e=>e.Employees)
                .SelectMany(e => e.Employees)  
                .ToListAsync();
        }
    }
}
