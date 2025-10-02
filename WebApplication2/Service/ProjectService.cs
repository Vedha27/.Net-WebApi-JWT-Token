using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebApplication2.MDbContext;
using WebApplication2.Models;
using WebApplication2.Repositories;

namespace WebApplication2.Service
{
    public class ProjectService : IProjectRepository
    {

        private readonly MyDbContext _myDbContext;

        public Task<IEnumerable<Employee>> GetYourEmployees { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ProjectService(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }
        public async Task<List<Project>> GetAllProjects()
        {
            return await _myDbContext.Projects.ToListAsync();
        }

        public async Task<Project> AddProject(Project project)
        {
          var demo= await _myDbContext.Projects.AddAsync(project);
            
          await _myDbContext.SaveChangesAsync();

          var res =await GetProjectById(project.ProjectId);

          return res;
        }

        public async Task<Project> DeleteProject(int id)
        {
            
            var res = await _myDbContext.Projects.FirstOrDefaultAsync(x=>x.ProjectId == id);
            if(res==null)
            {
                return null;
            }
            _myDbContext.Projects.Remove(res);
            await _myDbContext.SaveChangesAsync();
            
            return res;
        }

        public async Task<Project> GetProjectById(int id)
        {
            var pro= await _myDbContext.Projects.FirstOrDefaultAsync(x=>x.ProjectId==id);

            if(pro==null)
            {
                return null;
            }

            return pro;
        }

        public async Task<Project> UpadeProject(int pId, Project project)
        {
            var p = await _myDbContext.Projects.FirstOrDefaultAsync(x => x.ProjectId == pId);

            if(p==null)
            {
                return null;
            }

            p.ProjectName = project.ProjectName;

           await _myDbContext.SaveChangesAsync();

           return p;
        }
    }
}
