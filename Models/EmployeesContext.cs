using Microsoft.EntityFrameworkCore;

namespace SmallApi.Models
{
    public class EmployeesContext : DbContext
    {
        public EmployeesContext(DbContextOptions<EmployeesContext> options) : base(options)
        {

        }

        public DbSet<Employees> Employees { get; set; }
    }   
}
