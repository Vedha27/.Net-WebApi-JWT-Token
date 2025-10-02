using Microsoft.EntityFrameworkCore;
using WebApplication2.MDbContext;
using WebApplication2.Models;
using WebApplication2.Repositories;

namespace WebApplication2.Service
{
    public class DepartmentService : IDepartmentRepository
    {
        private readonly MyDbContext _dbContext;

        public DepartmentService(MyDbContext con)
        {
            _dbContext = con;
        }
        public async Task<List<Department>> GetAllDepartments()
        {
           var deptList = await _dbContext.Departments.ToListAsync();

           return  deptList;
        }

        public async Task<Department> GetDepartmentById(int id)
        {
            var existDept = await _dbContext.Departments.FirstOrDefaultAsync(x => x.DepartmentId==id);

            if(existDept == null)
            {
                return null;
            }

            return existDept;
        }
        

        public async Task<Department> AddDepartment(Department department)
        {
           await _dbContext.AddAsync(department);
           await _dbContext.SaveChangesAsync();

           return department;
        }

        public async Task<Department> UpdateDepartment(int id, Department department)
        {
            var existDept = await _dbContext.Departments.FirstOrDefaultAsync(x => x.DepartmentId == id);

            if( existDept == null)
            {
                return null;
            }

            existDept.Name = department.Name;
           await _dbContext.SaveChangesAsync();

           return existDept;
        }


        public async Task<Department> DeleteDepartment(int id)
        {
            var existDept = await _dbContext.Departments.FirstOrDefaultAsync( x=> x.DepartmentId == id);

            if (existDept == null)
            {
                return null;
            }

           _dbContext.Remove(existDept);
           await _dbContext.SaveChangesAsync();

           return existDept;
        }
    }
}
