using System.ComponentModel.DataAnnotations;

namespace WebApplication2.DTO
{
    public class EmployeeDto
    {
        [Required]
        [MinLength(3,ErrorMessage ="Name Shoul Has More than 3 Characters")]
        public string Name { get; set; }
        public int Salary { get; set; }
        public int DepartmentId { get; set; }
    }
}
