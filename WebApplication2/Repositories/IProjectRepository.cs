using WebApplication2.Models;

namespace WebApplication2.Repositories
{
    public interface IProjectRepository
    {

        Task<List<Project>> GetAllProjects();

        Task<Project> GetProjectById(int id);
        
        Task<Project> AddProject(Project project);
        Task<Project> UpadeProject(int pId, Project project);

        Task<Project> DeleteProject(int id);

        
    }
}
