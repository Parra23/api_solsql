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
    public class ReactionsController : ControllerBase
    {
        private readonly ContextDB _context;

        public ReactionsController(ContextDB context)
        {
            _context = context;
        }

        // GET: api/Reactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reactions>>> Getreactions()
        {
            try
            {
                var reactions = await _context.reactions
                    .FromSqlRaw("CALL pa_get_all_reactions()")
                    .ToListAsync();

                return Ok(reactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener las reacciones: {ex.Message}");
            }
        }

        // GET: api/Reactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reactions>> GetReactions(int id)
        {
            try
            {
                var reactions = await _context.reactions
                    .FromSqlInterpolated($"CALL pa_get_reaction_by_id({id})")
                    .ToListAsync();

                var reaction = reactions.FirstOrDefault();

                if (reaction == null)
                {
                    return NotFound($"No se encontró la reaccion con ID {id}.");
                }

                return Ok(reaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener la reaccion: {ex.Message}");
            }
        }

        // PUT: api/Reactions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReactions(int id, Reactions reactions)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL sp_update_reactions({id}, {reactions.reaction_type})"
                );

                return Ok(new { mensaje = "Reacción actualizada correctamente." });
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al actualizar la reacción.",
                    error = dbEx.InnerException?.Message ?? dbEx.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error inesperado al actualizar la reacción.",
                    error = ex.Message
                });
            }
        }

        // POST: api/Reactions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostReactions([FromBody] Reactions reactions)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($"CALL sp_insert_reactions({reactions.user_id}, {reactions.place_id}, {reactions.reaction_type})");

                return Ok(new
                {
                    mensaje = "Reacción registrada correctamente."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al registrar la reacción.",
                    error = ex.Message
                });
            }
        }

        // DELETE: api/Reactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReactions(int id)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL sp_delete_reactions({id})"
                );
                return Ok(new
                {
                    mensaje = "Reacción eliminada correctamente."
                });
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error al eliminar la reacción.",
                    error = dbEx.InnerException?.Message ?? dbEx.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "Error inesperado al eliminar la reacción.",
                    error = ex.Message
                });
            }
        }

        // GET: api/Reactions/user/{userId}/place/{placeId}
        [HttpGet("user/{userId}/place/{placeId}")]
        public async Task<ActionResult<Reactions>> GetReactionByUserAndPlace(int userId, int placeId)
        {
            try
            {
                var result = await _context.reactions
                    .FromSqlInterpolated($"CALL pa_get_reaction_by_idUser_idPlace({userId}, {placeId})")
                    .ToListAsync();

                var reaction = result.FirstOrDefault();

                if (reaction == null)
                {
                    return NotFound(new { message = "Reaccion no encontrado para el usuario y lugar especificados." });
                }

                return Ok(reaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "Error al obtener la reaccion.",
                    detail = ex.Message
                });
            }
        }

        // DELETE: api/Reactions/usuario/5/lugar/4
        [HttpDelete("usuario/{idUsuario}/lugar/{idLugar}")]
        public async Task<IActionResult> DeleteReaction(int idUsuario, int idLugar)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL pa_delete_reaction_by_idUser_idPlace({idUsuario}, {idLugar})"
                );

                return NoContent(); // 204
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new
                {
                    error = "No se pudo eliminar la reaccion.",
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

        private bool ReactionsExists(int id)
        {
            return _context.reactions.Any(e => e.id == id);
        }
    }
}
