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
    public class CitiesController : ControllerBase
    {
        private readonly ContextDB _context;

        public CitiesController(ContextDB context)
        {
            _context = context;
        }
        // GET: api/cities/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cities>>> GetCities()
        {
            try
            {
                var citiesList = await _context.cities
                    .FromSqlRaw("CALL sp_get_all_cities()")
                    .ToListAsync();
                return Ok(citiesList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Unexpected error", error = ex.Message });
            }
        }
        // GET: api/cities/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Cities>> GetCitiesByDepartment(int id)
        {
            try
            {
                var city = await _context.cities
                    .FromSqlInterpolated($"CALL sp_get_citysdept_by_id({id})")
                    .ToListAsync();
                return Ok(city);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Unexpected error", error = ex.Message });
            }
        }
        // POST: api/cities
        [HttpPost]
        public async Task<IActionResult> PostCity([FromBody] Cities city)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL sp_insert_cities({city.Name}, {city.department_id})"
                );
                return Ok(new { message = "Successfully inserted registration." });
            }
            catch (MySqlException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        // PUT: api/cities/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCity(int id, [FromBody] Cities city)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL sp_update_cities({id}, {city.Name}, {city.department_id})"
                );
                return Ok(new { message = "Successfully updated registration." });
            }
            catch (MySqlException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        // DELETE: api/cities/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL sp_delete_cities({id})"
                );
                return Ok(new { message = "Successfully deleted registration." });
            }
            catch (MySqlException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}