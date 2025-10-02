using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class Project
    {

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProjectId { get; set; }

        public string ProjectName { get; set; }

        public  virtual  List<Employee> Employees { get; set; }
    }
}
