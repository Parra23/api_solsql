using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_solsql.Context;
using api_solsql.Models;

namespace api_solsql.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly ContextDB _context;

        public FavoritesController(ContextDB context)
        {
            _context = context;
        }

        // GET: api/Favorites
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Favorites>>> Getfavorites()
        {
            try
            {
                var favorites = await _context.favorites
                    .FromSqlRaw("CALL pa_get_all_favorites()")
                    .ToListAsync();

                return Ok(favorites);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener los favoritos: {ex.Message}");
            }
        }

        // GET: api/Favorites/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Favorites>> GetFavorites(int id)
        {
            try
            {
                var favorites = await _context.favorites
                    .FromSqlInterpolated($"CALL pa_get_favorite_by_id({id})")
                    .ToListAsync();

                var favorite = favorites.FirstOrDefault();

                if (favorite == null)
                {
                    return NotFound($"No se encontró el favorito con ID {id}.");
                }

                return Ok(favorite);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener el favorito: {ex.Message}");
            }
        }


        // GET: api/Favorites/byUser/3
        [HttpGet("byUser/{user_id}")]
        public async Task<ActionResult<IEnumerable<Favorites>>> GetFavoritesPorUsuario(int user_id)
        {
            try
            {
                var favorites = await _context.favorites
                    .FromSqlInterpolated($"CALL pa_get_favorite_by_user({user_id})")
                    .ToListAsync();

                return Ok(favorites);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener el favorito: {ex.Message}");
            }
        }

        // GET: api/Favorites/user/{userId}/place/{placeId}
        [HttpGet("user/{userId}/place/{placeId}")]
        public async Task<ActionResult<Favorites>> GetFavoriteByUserAndPlace(int userId, int placeId)
        {
            try
            {
                var result = await _context.favorites
                    .FromSqlInterpolated($"CALL pa_get_favorite_by_user_place({userId}, {placeId})")
                    .ToListAsync();

                var favorite = result.FirstOrDefault();

                if (favorite == null)
                {
                    return NotFound(new { message = "Favorito no encontrado para el usuario y lugar especificados." });
                }

                return Ok(favorite);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "Error al obtener el favorito.",
                    detail = ex.Message
                });
            }
        }


        //// PUT: api/Favorites/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutFavorites(int id, Favorites favorites)
        //{
        //    if (id != favorites.favorite_id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(favorites).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!FavoritesExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        [HttpPost]
        public async Task<IActionResult> PostFavorites(Favorites favorites)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL sp_insert_favorites({favorites.id}, {favorites.place_id})"
                );

                return Ok(new { message = "Favorito agregado correctamente." });
            }
            catch (DbUpdateException ex)
            {
                // Captura errores que vienen de la base de datos (incluido SIGNAL SQLSTATE)
                return BadRequest(new
                {
                    error = "No se pudo agregar el favorito.",
                    detail = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "Error interno del servidor.",
                    detail = ex.Message
                });
            }
        }


        // DELETE: api/Favorites/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFavorites(int id)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL sp_delete_favorites({id})"
                );

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new
                {
                    error = "No se pudo eliminar el favorito.",
                    detail = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "Error interno del servidor.",
                    detail = ex.Message
                });
            }
        }

        // DELETE: api/Favorites/usuario/5/lugar/4
        [HttpDelete("usuario/{idUsuario}/lugar/{idLugar}")]
        public async Task<IActionResult> DeleteFavorite(int idUsuario, int idLugar)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL pa_delete_favorite_by_user_place({idUsuario}, {idLugar})"
                );

                return NoContent(); // 204
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new
                {
                    error = "No se pudo eliminar el favorito.",
                    detail = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "Error interno del servidor.",
                    detail = ex.Message
                });
            }
        }



        private bool FavoritesExists(int id)
        {
            return _context.favorites.Any(e => e.favorite_id == id);
        }
    }
}
