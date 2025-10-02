using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.DTO;
using WebApplication2.Models;
using WebApplication2.Repositories;

namespace WebApplication2.Controllers
{
    [Route("api/projects")]
    [ApiController]
    public class ProjectController : ControllerBase
    {

        private IProjectRepository _projectRepos;

        public ProjectController(IProjectRepository projectRepos)
        {
            _projectRepos = projectRepos;
        }

        [HttpGet]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetAllProjects()
        {
            var domainProjectList = await _projectRepos.GetAllProjects();

            var dtoProjectList = new List<ProjectDTO>();

            foreach(var p in domainProjectList)
            {
                dtoProjectList.Add(new ProjectDTO() { ProjectId = p.ProjectId, ProjectName = p.ProjectName });
            }

            return Ok(dtoProjectList);
        }

        [HttpGet]
        [Route("{id:int}")]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetProjectById([FromRoute]int id)
        {
            var domainProject= await _projectRepos.GetProjectById(id);
            if (domainProject == null)
            {
                return NotFound();
            }
            
            var dtoProject= new ProjectDTO() {ProjectId = id,ProjectName=domainProject.ProjectName};

            return Ok(dtoProject);
        }

        [HttpPost]
        [Route("add")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> AddProject([FromBody]ProjectDTO dto)
        {
            var domainProject=new Project() {  ProjectId = dto.ProjectId,ProjectName=dto.ProjectName};

            await _projectRepos.AddProject(domainProject);

            return Ok("Project Inserted Succesffully");
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateProject([FromRoute]int id,[FromBody]ProjectDTO dtoProject)
        {
            var domainProject = new Project() { ProjectId=dtoProject.ProjectId,ProjectName=dtoProject.ProjectName};

            domainProject = await _projectRepos.UpadeProject(id, domainProject);

            if(domainProject==null) { return NotFound(); }

            dtoProject = new ProjectDTO() { ProjectId = domainProject.ProjectId, ProjectName = domainProject.ProjectName};

            return Ok(dtoProject);

        }

        [HttpDelete]
        [Route("delete/{id:int}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteProject([FromRoute]int id)
        {
            var domainProject= await _projectRepos.DeleteProject(id);

            if(domainProject==null)
            {
                return NotFound();
            }

            var dtoProject = new ProjectDTO() { ProjectId=domainProject.ProjectId,ProjectName= domainProject.ProjectName};

            return Ok(dtoProject);

        }
    }
}
