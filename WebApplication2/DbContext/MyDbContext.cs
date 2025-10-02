using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.MDbContext
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }

        public DbSet<Project> Projects { get; set; }
    }

}
