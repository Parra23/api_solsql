using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_solsql.Context;
using api_solsql.Models;

namespace api_solsql.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly ContextDB _context;

        public PhotosController(ContextDB context)
        {
            _context = context;
        }

        // GET: api/Photos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Photos>>> Getphotos()
        {
            var photos = await _context.photos
                .FromSqlRaw("CALL pa_get_all_photos()")
                .ToListAsync();

            return photos;
        }


        // GET: api/Photos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Photos>> GetPhotos(int id)
        {
            try
            {
                var photos = await _context.photos
                    .FromSqlRaw("CALL pa_get_photo_by_id({0})", id)
                    .ToListAsync();

                var photo = photos.FirstOrDefault();

                if (photo == null)
                {
                    return NotFound(new { message = "La foto no existe" });
                }

                return photo;
            }
            catch (Exception ex)
            {
                // Puedes registrar el error aquí si usas un sistema de logs
                return StatusCode(500, new { message = "Error al obtener la foto", detail = ex.Message });
            }
        }


        // GET: api/Photos/place/3
        [HttpGet("place/{p_place_id}")]
        public async Task<ActionResult<IEnumerable<Photos>>> GetPhotosByPlace(int p_place_id)
        {
            try
            {
                var fotos = await _context.photos
                    .FromSqlInterpolated($"CALL pa_fotos_lugar({p_place_id})")
                    .ToListAsync();

                if (fotos == null || fotos.Count == 0)
                {
                    return NotFound("No hay fotos para este lugar.");
                }

                return fotos;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al ejecutar el procedimiento almacenado: {ex.Message}");
            }
        }


        // PUT: api/Photos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhotos(int id, [FromBody] Photos photos)
        {
            try
            {
                int result = await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL sp_update_photos({id}, {photos.url}, {photos.description})"
                );

                return Ok(new { message = "Successfully updated registration." });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"Error al actualizar la foto: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error inesperado: {ex.Message}");
            }
        }


        // POST: api/Photos
        [HttpPost]
        public async Task<IActionResult> PostPhotos(Photos photos)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL sp_insert_photos({photos.place_id}, {photos.url}, {photos.description})"
                );

                return Ok(new { message = "Successfully inserted registration." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al insertar la foto: {ex.Message}");
            }
        }


        // DELETE: api/Photos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhotos(int id)
        {
            try
            {
                var result = await _context.Database.ExecuteSqlRawAsync("CALL sp_delete_photos({0})", id);
                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                //  si la foto no existe
                return NotFound(new { message = "La foto no existe" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", detail = ex.Message });
            }
        }


        private bool PhotosExists(int id)
        {
            return _context.photos.Any(e => e.photo_id == id);
        }
    }
}
