using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.DTO;
using WebApplication2.Models;
using WebApplication2.Repositories;
using WebApplication2.Service;
namespace WebApplication2.Controllers
{
    [Route("api")]
    [ApiController]
    public class EmployeeProjectController : ControllerBase
    {

        private readonly IEmployeeProjectRepository _repo;
        public EmployeeProjectController(IEmployeeProjectRepository rep) => _repo = rep;

        [HttpGet("employee/{employeeId:int}/projects")]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetProjectsByEmployee([FromRoute]int employeeId)
        { 
               
            var domain = await _repo.GetProjectsByEmployeeIdAsync(employeeId);

            var res = domain.Select(p => new ProjectDTO()
            {
                ProjectId = p.ProjectId,
                ProjectName = p.ProjectName
            }).ToList();

            return Ok(res);
        }


        [HttpGet("project/{projectId:int}/employees")]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> GetEmployeesByProject([FromRoute]int projectId)
        {
            var domain = await _repo.GetEmployeesByProjectIdAsync(projectId);

            var res = domain.Select(e => new EmployeeDTOForShowing()
            {
                Id = e.Id,
                Name = e.Name,
                DepartmentId=e.DepartmentId,
                Salary=e.Salary
            }).ToList();

            return Ok(res);
        }
      
     }
}
