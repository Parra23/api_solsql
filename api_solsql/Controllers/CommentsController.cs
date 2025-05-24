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
    public class CommentsController : ControllerBase
    {
        private readonly ContextDB _context;

        public CommentsController(ContextDB context)
        {
            _context = context;
        }


        // GET: api/Comments/by_lugar/3
        [HttpGet("byLugar/{place_id}")]
        public async Task<ActionResult<IEnumerable<CommentsPlace>>> GetComentariosPorLugar(int place_id)
        {
            try
            {
                var comentarios = await _context.Set<CommentsPlace>()
                    .FromSqlInterpolated($"CALL pa_ver_comentarios_lugar({place_id})")
                    .ToListAsync();

                if (comentarios == null || !comentarios.Any())
                {
                    return NotFound($"No se encontraron comentarios para el lugar con ID {place_id}.");
                }

                return Ok(comentarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener los comentarios del lugar: {ex.Message}");
            }
        }



        // GET: api/Comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comments>>> Getcomments()
        {
            try
            {
                var comments = await _context.comments
                    .FromSqlRaw("CALL pa_get_all_comments()")
                    .ToListAsync();

                return Ok(comments);
            }
            catch (Exception ex)
            {
                // Puedes registrar el error con un logger si tienes uno configurado
                return StatusCode(500, $"Error al obtener los comentarios: {ex.Message}");
            }
        }


        // GET: api/Comments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Comments>> GetComments(int id)
        {
            try
            {
                var comments = await _context.comments
                    .FromSqlInterpolated($"CALL pa_get_comment_by_id({id})")
                    .ToListAsync();

                var comment = comments.FirstOrDefault();

                if (comment == null)
                {
                    return NotFound($"No se encontró el comentario con ID {id}.");
                }

                return Ok(comment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener el comentario: {ex.Message}");
            }
        }


        // PUT: api/Comments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComments(int id, [FromBody]Comments comments)
        {
            try
            {
                var result = await _context.Database.ExecuteSqlInterpolatedAsync($@"
            CALL pa_update_comment(
                {id},
                {comments.comment}
            )");

                if (result == 0)
                {
                    return NotFound($"No se encontró el comentario con ID {id} para actualizar.");
                }
                return Ok($"Comentario con ID {id} actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar el comentario: {ex.Message}");
            }
        }


        // POST: api/Comments
        [HttpPost]
        public async Task<ActionResult<Comments>> PostComments([FromBody] Comments comments)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($@"
            CALL pa_insert_comment(
                {comments.place_id},
                {comments.id},
                {comments.comment},
                {comments.parent_comment_id},
                {comments.comment_date}
            )");

                return Ok("Comentario insertado correctamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al insertar el comentario: {ex.Message}");
            }
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComments(int id)
        {
            try
            {
                var result = await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL pa_delete_comment({id})"
                );
                if (result == 0)
                {
                    return NotFound($"No se encontró el comentario con ID {id} para eliminar.");
                }

                return Ok($"Comentario con ID {id} eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar el comentario: {ex.Message}");
            }
        }


        private bool CommentsExists(int id)
        {
            return _context.comments.Any(e => e.comment_id == id);
        }
    }
}
