using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApplication2.DTO;
using WebApplication2.Models;
using WebApplication2.Repositories;

namespace WebApplication2.Controllers
{
    [Route("api/departments")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _deptRepo;
        public DepartmentController(IDepartmentRepository repo)
        {
            _deptRepo = repo;
        }

        [HttpGet]
        [Authorize(Roles ="Reader,Writer")]
        public async Task<IActionResult> GetAllDepartments()
        {
            var domainDeptList= await _deptRepo.GetAllDepartments();

            var dtoDeptList = new List<DepartmentDTO>();

            foreach(var d in domainDeptList)
            {

                dtoDeptList.Add(new DepartmentDTO() { DepartmentId=d.DepartmentId,Name=d.Name});
            }

            return Ok(dtoDeptList);
        }

        [HttpGet]
        [Route("{id:int}")]
        [Authorize(Roles ="Reader,Writer")]
        public async Task<IActionResult> GetDepartmentById([FromRoute]int id)
        {
            var domainDept = await _deptRepo.GetDepartmentById(id);

            if(domainDept==null)
            {
                return NotFound();
            }

            var dtoDept = new DepartmentDTO() { DepartmentId = id,Name=domainDept.Name };

            return Ok(dtoDept);
        }
       

        [HttpPost]
        [Route("add")]
        [Authorize(Roles ="Writer")]
        public async Task<IActionResult> AddDepartment([FromBody] DepartmentDTO dept)
        {
            var domainDep = new Department() {DepartmentId=dept.DepartmentId, Name = dept.Name };
            var res = await _deptRepo.AddDepartment(domainDep);

            return Ok("data inserted");
        }

        [HttpPost]
        [Route("update/{id:int}")]
        [Authorize(Roles ="Writer")]
        public async Task<IActionResult> UpdateDepartment([FromRoute] int id, [FromBody] DepartmentDTO dtoDept)
        {

            var domainDept = new Department() { DepartmentId=id,Name=dtoDept.Name};

            domainDept=await _deptRepo.UpdateDepartment(id, domainDept);

            if(domainDept==null)
            {
                return NotFound();
            }

            return Ok(new DepartmentDTO() { DepartmentId = id,Name=domainDept.Name});
        }

        [HttpDelete]
        [Route("delete/{id:int}")]
        [Authorize(Roles ="Writer")]
        public async Task<IActionResult> DeleteDepartment([FromRoute]int id)
        {
            var domainDept=await _deptRepo.DeleteDepartment(id);


            return domainDept == null ? NotFound() : Ok(new DepartmentDTO() {DepartmentId=id,Name=domainDept.Name});
        }

    }
}
