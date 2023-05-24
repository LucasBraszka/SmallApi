using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmallApi.Models;

namespace SmallApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        private readonly EmployeesContext _dbContext;

        public EmployeeController(EmployeesContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employees>>> GetEmployees()
        {
            if (_dbContext.Employees == null)
            {
                return NotFound();
            }
            return await _dbContext.Employees.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employees>> GetEmployee(int id)
        {
            if (_dbContext.Employees == null)
            {
                return NotFound();
            }

            var brand = await _dbContext.Employees.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            return brand;
        }

        [HttpPost]

        public async Task<ActionResult<Employees>> PostEmployee(Employees employee)
        {
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployees), new { id = employee.Id }, employee);
        }

        [HttpPut]

        public async Task<IActionResult> PutBrand(int id, Employees employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }
            _dbContext.Entry(employee).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeAvailable(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }

        [HttpGet("brand-available/{id}")]
        public bool EmployeeAvailable(int id)
        {
            return (_dbContext.Employees?.Any(x => x.Id == id)).GetValueOrDefault();
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteBrand(int id)
        {
            if (_dbContext.Employees == null)
            {
                return NotFound();
            }

            var brand = await _dbContext.Employees.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            _dbContext.Employees.Remove(brand);

            await _dbContext.SaveChangesAsync();

            return Ok();
        }

    }
}
