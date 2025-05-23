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
    public class PlaceTypesController : ControllerBase
    {
        private readonly ContextDB _context;

        public PlaceTypesController(ContextDB context)
        {
            _context = context;
        }

        // GET: api/placetypes/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlaceTypes>>> GetPlaceTypes()
        {
            try
            {
                var placeTypesList = await _context.placeTypes
                    .FromSqlRaw("CALL sp_get_all_placetypes()")
                    .ToListAsync();
                return Ok(placeTypesList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Unexpected error", error = ex.Message });
            }
        }
        // GET: api/placetypes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PlaceTypes>> GetPlaceTypeById(int id)
        {
            try
            {
                var placeType = await _context.placeTypes
                    .FromSqlInterpolated($"CALL sp_get_placetypes_by_id({id})")
                    .ToListAsync();
                return Ok(placeType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Unexpected error", error = ex.Message });
            }
        }

        // POST: api/placetypes
        [HttpPost]
        public async Task<IActionResult> PostPlaceType([FromBody] PlaceTypes placeType)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL sp_insert_placetypes({placeType.Name}, {placeType.Description})"
                );
                return Ok(new { message = "Place type created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Unexpected error", error = ex.Message });
            }
        }

        // PUT: api/placetypes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlaceType(int id, [FromBody] PlaceTypes placeType)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL sp_update_placetypes({id}, {placeType.Name}, {placeType.Description})"
                );
                return Ok(new { message = "Place type updated successfully" });
            }
            catch (MySqlException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
        // DELETE: api/placetypes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlaceType(int id)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL sp_delete_placetypes({id})"
                );
                return Ok(new { message = "Place type deleted successfully" });
            }
            catch (MySqlException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
