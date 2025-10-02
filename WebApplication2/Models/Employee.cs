namespace WebApplication2.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Salary {  get; set; }
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        public virtual List< Project> Projects{ get; set; }

    }
}
