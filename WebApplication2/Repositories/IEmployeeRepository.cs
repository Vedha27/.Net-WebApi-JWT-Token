using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
namespace WebApplication2.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllEmployeesAsync(string filterOn, string filterBy , string sortBy,
                    bool isAscending = true, int pageNumber=1, int pageSize=100);

        Task<Employee> GetEmployeeByIdAsync(int id);

        Task<Employee> AddEmployeeAsync(Employee e);

        Task<Employee> UpdateEmployeeAsync(int id,Employee e);
        
        Task<Employee> DeleteEmployeeAsync(int id);

        Task <string> AssignProjectToEmployee(int eId, int pId);
    }
}
