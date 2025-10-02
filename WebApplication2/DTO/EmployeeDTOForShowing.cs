using WebApplication2.Models;

namespace WebApplication2.DTO
{
    public class EmployeeDTOForShowing
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Salary { get; set; }
        public int DepartmentId { get; set; }

        public DepartmentDTO Department { get; set; }
        public List<ProjectDTO> Projects { get; set; }
    }
}
