using WebApplication2.Models;

namespace WebApplication2.Repositories
{
    public interface IEmployeeProjectRepository
    {
        Task<List<Project>> GetProjectsByEmployeeIdAsync(int employeeId);
        Task<List<Employee>> GetEmployeesByProjectIdAsync(int projectId);


        //Task UnassignProjectFromEmployeeAsync(int employeeId, int projectId);

    }
}
