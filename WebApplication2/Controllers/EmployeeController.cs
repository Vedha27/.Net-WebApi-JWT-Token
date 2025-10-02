using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.DTO;
using WebApplication2.Models;
using WebApplication2.Repositories;
using WebApplication2.ValidateFolder;
using SerilogTimings;
using WebApplication2.Filters.ActionFilters;
using WebApplication2.Filters.ResourceFitlers;

namespace WebApplication2.Controllers
{
    [Route("api/employees")]
    [ApiController]
   public class EmployeeController : ControllerBase
    {
       private readonly IEmployeeRepository _employeeRepo;

        private readonly ILogger<EmployeeController> _logger;
        
        public EmployeeController(IEmployeeRepository repo,ILogger<EmployeeController> logger) 
        {
            this._employeeRepo=repo;
            _logger = logger;
        }

        //Get All Employees
        [HttpGet]
        [Authorize(Roles ="Reader,Writer")]
        [TypeFilter(typeof(MyActionFilter))]
        public async Task<IActionResult> GetAllEmloyees([FromQuery]string? filterOn,[FromQuery]string? filterBy,
                [FromQuery]string? sortBy, [FromQuery]bool isAscending, [FromQuery]int pageNumber=1, [FromQuery]int pageSize=100)
        {
            using (Operation.Time("Time for Filtered Employee"))
            {
                try
                {
                    _logger.LogInformation("Fetching employees with FilterOn={FilterOn}, FilterBy={FilterBy}, SortBy={SortBy}," +
                        " IsAscending={IsAscending}, PageNumber={PageNumber}, PageSize={PageSize}",
                    filterOn, filterBy, sortBy, isAscending, pageNumber, pageSize);
                    var domainEmpList = await _employeeRepo.GetAllEmployeesAsync(filterOn, filterBy, sortBy, isAscending, pageNumber, pageSize);

                    var dtoEmpList = new List<EmployeeDTOForShowing>();

                    foreach (var e in domainEmpList)
                    {
                        dtoEmpList.Add(new EmployeeDTOForShowing()
                        {
                            Id = e.Id,
                            Name = e.Name,
                            Salary = e.Salary,
                            DepartmentId = e.DepartmentId,
                            Department = new DepartmentDTO() { DepartmentId = e.DepartmentId, Name = e.Department.Name },
                            Projects = e.Projects.Select(p => new ProjectDTO { ProjectId = p.ProjectId, ProjectName = p.ProjectName }).ToList()

                        });
                    }

                    _logger.LogInformation("Successfully retrieved {Count} employees.", dtoEmpList.Count);

                    return Ok(dtoEmpList);
                }

                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while fetching employees.");
                    return StatusCode(500, "Internal server error. Please try again later.");
                }
            }
          }

        //Get Employee By Id
        [HttpGet]
        [Route("{id:int}")]
        [Authorize(Roles ="Reader,Writer")]
        public async Task<IActionResult> EmployeedById([FromRoute] int id)
        {
            var domainEmp = await _employeeRepo.GetEmployeeByIdAsync(id);

            if (domainEmp == null)
            {
                return NotFound();
            }

            var dtoEmp = new EmployeeDTOForShowing() {
                Id = id, 
                Name = domainEmp.Name,
                DepartmentId = domainEmp.DepartmentId, 
                Salary = domainEmp.Salary 
            };

            return Ok(dtoEmp);
        }

        [HttpPost]
        [Route("add")]
        [ValidateModel]
        [Authorize(Roles ="Writer")]
        [TypeFilter(typeof(MyResourceFilter),Arguments =new Object[] {false})]
        public  async Task<IActionResult> AddEmployee([FromBody] EmployeeDto emp)
        {
            //Convert to Domain
            var domainEmp = new Employee()
            {
                Name = emp.Name,
                DepartmentId = emp.DepartmentId,
                Salary = emp.Salary
            };

            await _employeeRepo.AddEmployeeAsync(domainEmp);

            var dtoEmp = new EmployeeDTOForShowing()
            {
                Id = domainEmp.Id,
                Name = domainEmp.Name,
                DepartmentId = domainEmp.DepartmentId,
                Salary = domainEmp.Salary
            };

            return CreatedAtAction(nameof(EmployeedById), new { domainEmp.Id }, dtoEmp);
        }
        
        //Update
        [HttpPut]
        [Route("pdate/{id:int}")]
        [Authorize(Roles ="Writer")]
        public async Task<IActionResult> UpdateEmployee([FromRoute]int id,[FromBody] EmployeeDto emp)
        {
            var domainEmp = new Employee()
            {
                Name = emp.Name,
                DepartmentId = emp.DepartmentId,
                Salary = emp.Salary

            };
            
            var empModel = await _employeeRepo.UpdateEmployeeAsync(id,domainEmp);

            if(empModel ==null)
            {
                return NotFound();
            }

            empModel.Name = emp.Name;
            empModel.DepartmentId = emp.DepartmentId;
            empModel.Salary = emp.Salary;

            return Ok();
        }

        //Delete
        [HttpDelete]
        [Route("delete/{id:int}")]
        [Authorize(Roles ="Writer")]
        public async Task<IActionResult> DeleteEmployee([FromRoute]int id)
        {
            var empModel = await _employeeRepo.DeleteEmployeeAsync(id);

            if(empModel==null)
            {
                return NotFound();
            }

            return Ok(empModel);
        }

        [HttpPost]
        [Route("{eId:int}/assign-project/{pId:int}")]
        [Authorize(Roles ="Writer")]
        public async Task<IActionResult> AddProjectToEmloyee([FromRoute]int eId, [FromRoute] int pId)
        {
          var res = await  _employeeRepo.AssignProjectToEmployee(eId, pId);

            return Ok(res);
        }

    }
}
