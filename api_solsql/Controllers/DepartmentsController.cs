using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_solsql.Context;
using api_solsql.Models;
using MySqlConnector;
namespace api_solsql.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly ContextDB _context;

        public DepartmentsController(ContextDB context)
        {
            _context = context;
        }
        // GET: api/departments/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Departments>>> GetDepts()
        {
            try
            {
                var deptsList = await _context.departments
                    .FromSqlRaw("CALL sp_get_all_dept()")
                    .ToListAsync();
                return Ok(deptsList);
            }
            catch (MySqlException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        // GET: api/departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Departments>> Getdepts(int id)
        {
            try
            {
                var depts = await _context.departments
                    .FromSqlInterpolated($"CALL sp_get_dept_by_id({id})")
                    .ToListAsync();
                return Ok(depts);
            }
            catch (MySqlException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        // POST: api/depts
        [HttpPost]
        public async Task<IActionResult> Postdepts([FromBody] Departments dept)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL sp_insert_dept({dept.Name})"
                );
                return Ok(new { message = "Successfully inserted registration." });
            }
            catch (MySqlException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        // PUT: api/depts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Putdepts(int id, [FromBody] Departments dept)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL sp_update_dept({id}, {dept.Name})"
                );
                return Ok(new { message = "Successfully updated registration." });
            }catch (MySqlException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        // DELETE: api/depts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletedpts(int id)
        {
            try
            {
                var depts_delete = await _context.Database
                    .ExecuteSqlInterpolatedAsync($"CALL sp_delete_dept({id})");
                return Ok(depts_delete);
            }catch (MySqlException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
