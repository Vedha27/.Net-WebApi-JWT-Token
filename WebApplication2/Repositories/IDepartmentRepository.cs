using WebApplication2.MDbContext;
using WebApplication2.Models;

namespace WebApplication2.Repositories
{
    public interface IDepartmentRepository
    {

        Task<List<Department>> GetAllDepartments();

        Task<Department> GetDepartmentById(int id);

        Task<Department> AddDepartment(Department department);


        Task<Department> UpdateDepartment(int id, Department department);

        Task<Department> DeleteDepartment(int id);

    }
}
