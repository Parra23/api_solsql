using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_solsql.Context;
using api_solsql.Models;

namespace api_solsql.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlacesController : ControllerBase
    {
        private readonly ContextDB _context;

        public PlacesController(ContextDB context)
        {
            _context = context;
        }

        // GET: api/Places/info_rapida
        [HttpGet("info_rapida")]
        public async Task<ActionResult<IEnumerable<vw_general_lugar>>> GetInfoGeneralLugares()
        {
            try
            {
                var lugares = await _context.vw_general_lugar
                    .FromSqlRaw("CALL pa_obtener_info_general_lugares()")
                    .ToListAsync();

                return Ok(lugares);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener la información general de lugares: {ex.Message}");
            }
        }

        // GET: api/Places/buscar
        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<vw_general_lugar>>> BuscarLugares([FromQuery] string busqueda)
        {
            if (string.IsNullOrWhiteSpace(busqueda))
            {
                return BadRequest("Debe proporcionar un término de búsqueda.");
            }

            try
            {
                var resultados = await _context.vw_general_lugar
                    .FromSqlInterpolated($"CALL pa_buscar_lugares({busqueda})")
                    .ToListAsync();

                return Ok(resultados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al buscar lugares: {ex.Message}");
            }
        }


        // GET: api/Places
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Places>>> Getplaces()
        {
            try
            {
                var places = await _context.places
                    .FromSqlRaw("CALL pa_get_all_places()")
                    .ToListAsync();

                return Ok(places);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener los lugares: {ex.Message}");
            }
        }

        // GET: api/Places/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Places>> GetPlaces(int id)
        {
            try
            {
                var places = await _context.places
                    .FromSqlInterpolated($"CALL pa_get_place_by_id({id})")
                    .ToListAsync();

                var place = places.FirstOrDefault();

                if (place == null)
                {
                    return NotFound($"No se encontró el lugar con ID {id}.");
                }

                return Ok(place);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener el lugar: {ex.Message}");
            }
        }


        // PUT: api/Places/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlaces(int id, [FromBody] Places places)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($@"
            CALL sp_update_places(
                {id},
                {places.name},
                {places.type_id},
                {places.description},
                {places.address},
                {places.city_id},
                {places.opening_hours},
                {places.fees},
                {places.coordinates},
                {places.contact_phone},
                {places.contact_email},
                {places.social_media},
                {places.status}
            );
        ");

                return Ok($"Lugar con ID {id} actualizado exitosamente.");
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, $"Error de base de datos al actualizar el lugar: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el lugar: {ex.Message}");
            }
        }


        // POST: api/Places
        [HttpPost]
        public async Task<IActionResult> PostPlaces([FromBody] Places places)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($@"
            CALL sp_insert_places(
                {places.name},
                {places.type_id},
                {places.description},
                {places.address},
                {places.city_id},
                {places.opening_hours},
                {places.fees},
                {places.coordinates},
                {places.contact_phone},
                {places.contact_email},
                {places.social_media}
            )
        ");
                return Ok("Lugar insertado exitosamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al insertar el lugar: {ex.Message}");
            }
        }

        // DELETE: api/Places/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlaces(int id)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($@"
            CALL sp_delete_places({id});
                ");

                return Ok($"Lugar con ID {id} eliminado exitosamente.");
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, $"Error en base de datos al eliminar el lugar: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar el lugar: {ex.Message}");
            }
        }


        // PUT: api/Places/deactivate/5
        [HttpPut("deactivate/{id}")]
        public async Task<IActionResult> DeactivatePlace(int id)
        {
            try
            {
                var result = await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL sp_deactivate_places({id})");

                return Ok($"Lugar con ID {id} desactivado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al desactivar el lugar: {ex.Message}");
            }
        }

        // PUT: api/Places/activate/5
        [HttpPut("activate/{id}")]
        public async Task<IActionResult> ActivatePlace(int id)
        {
            try
            {
                var result = await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL sp_activate_places({id})");

                return Ok($"Lugar con ID {id} activado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al activar el lugar: {ex.Message}");
            }
        }

        private bool PlacesExists(int id)
        {
            return _context.places.Any(e => e.place_id == id);
        }
    }
}
