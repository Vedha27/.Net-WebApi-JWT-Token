using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.MDbContext;
using WebApplication2.Models;
using WebApplication2.Repositories;

namespace WebApplication2.Service
{
    public class EmployeeService : IEmployeeRepository
    {
        private readonly MyDbContext _dbContext;

        public EmployeeService(MyDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<List<Employee>> GetAllEmployeesAsync(string? filterOn,string? filterBy, string? sortBy, bool isAscending = true,
                                                                 int pageNumber = 1,int pageSize = 1000)
        {
            var res = _dbContext.Employees.Include("Department").Include("Projects").AsQueryable();

         
            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterBy))
            {
                if (filterOn.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    res = res.Where(e => e.Name.Contains(filterBy));
                }
                else if (filterOn.Equals("DeptId", StringComparison.OrdinalIgnoreCase))
                {
                    if (int.TryParse(filterBy, out int id))
                    {
                        res = res.Where(e => e.DepartmentId == id);
                    }
                }
            }

          
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    res = isAscending ? res.OrderBy(x => x.Name) : res.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("salary", StringComparison.OrdinalIgnoreCase))
                {
                    res = isAscending ? res.OrderBy(x => x.Salary) : res.OrderByDescending(x => x.Salary);
                }
            }

            
            int skipResult = (pageNumber - 1) * pageSize;

            return await res.Skip(skipResult).Take(pageSize).ToListAsync();
        }


        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            var existEmp = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (existEmp == null)
            {
                return null;
            }

            return existEmp;
        }

        public async Task<Employee> AddEmployeeAsync(Employee e)
        {
            await _dbContext.Employees.AddAsync(e);
            await _dbContext.SaveChangesAsync();
           

            return e;
        }

        public async Task<Employee> UpdateEmployeeAsync(int id,Employee e)
        {
            var existEmp =  await _dbContext.Employees.FirstOrDefaultAsync(x=> x.Id==id);

            if (existEmp == null) 
            {
                return null;
            }
          
            existEmp.Name=e.Name;
            existEmp.Department = e.Department;
            existEmp.Salary = e.Salary;

            await _dbContext.SaveChangesAsync();

            return existEmp;
        }

        public async Task<Employee> DeleteEmployeeAsync(int id)
        {
            var existEmp = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (existEmp == null)
            {
                return null;
            }

            _dbContext.Employees.Remove(existEmp);

            await _dbContext.SaveChangesAsync();

            return existEmp;
        }

        public async Task<string> AssignProjectToEmployee(int eId, int pId)
        {
            var employee = await _dbContext.Employees.FindAsync(eId);
            var project = await _dbContext.Projects.FindAsync(pId);

            if (employee == null || project == null)
            {

                return "Employee or Project is Not Found";
            }

            if (employee.Projects == null)
            {
                employee.Projects = new List<Project>();
            }
            
            if (!employee.Projects.Any(p => p.ProjectId == pId))
            {
                employee.Projects.Add(project);
                await _dbContext.SaveChangesAsync();
                return "Assigned Successfully";
            }

            return " Failed to Assign ";
        }

        
    }
}
