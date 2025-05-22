using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_solsql.Context;
using api_solsql.Models;
using MySqlConnector;
using BcryptNet = BCrypt.Net.BCrypt;

namespace api_solsql.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class vw_userController : ControllerBase

    {
        private readonly ContextDB _context;

        public vw_userController(ContextDB context)
        {
            _context = context;
        }
        // GET: api/users/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<vw_user>>> Getusers() {
            try {
                var usersList = await _context.VW_users
                    .FromSqlRaw("CALL sp_get_all_users()")
                    .ToListAsync();
                return Ok(usersList);
            } catch (Exception ex) {
                return StatusCode(500,new { message = "Error al obtener los usuarios",error = ex.Message });
            }
        }
        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<vw_user>> Getusers(int id) {
            try {
                var users = await _context.VW_users
                    .FromSqlInterpolated($"CALL sp_get_user_by_id({id})")
                    .ToListAsync();

                if (users == null || users.Count == 0) {
                    return NotFound(new { message = "Usuario no encontrado" });
                }

                return Ok(users);
            } catch (Exception ex) {
                return StatusCode(500,new { message = "Error al obtener el usuario",error = ex.Message });
            }
        }
          // GET: api/users/status/{status}
            [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<vw_user>>> GetUsersByStatus(int status) {
            try {
                var usersList = new List<vw_user>();

                if (status == 1) {
                    usersList = await _context.VW_users
                        .FromSqlRaw("CALL sp_get_all_users_active()")
                        .ToListAsync();
                } else if (status == 0) {
                    usersList = await _context.VW_users
                        .FromSqlRaw("CALL sp_get_all_users_desactive()")
                        .ToListAsync();
                } else {
                    return BadRequest(new { message = "El estado debe ser activo (1) o inactivo (0)" });
                }

                    return Ok(usersList);
            } catch (Exception ex) {
                return StatusCode(500,new { message = "Error al obtener los usuarios",error = ex.Message });
            }
        }
        // GET: api/users/login
        [HttpGet("login/{email}/{password}")]
        public async Task<ActionResult<vw_user>> GetusersLogin(String email, String password)
        {
            try {
                var users = await _context.VW_users
                .FromSqlInterpolated($"CALL sp_login({email}, {password})")
                .ToListAsync();
                return Ok(users);
            } catch (MySqlException ex) {
                // Error específico de MySQL (requiere: using MySql.Data.MySqlClient)
                return StatusCode(500,new {error = ex.Message });
            } catch (Exception ex) {
                // Otro tipo de error
                return StatusCode(500,new { message = "Error inesperado",error = ex.Message });
            }
        }
         // PUT: api/users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
       [HttpPut("{id}")]
        public async Task<IActionResult> Putusers(int id, vw_user user)
        {
            try {
                // Verifica si la contraseña ya está hasheada o si es una nueva
                if (!user.Password.StartsWith("$2a$")) {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                }

                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL sp_update_user({id}, {user.Name}, {user.Email}, {user.Role}, {user.Password}, {user.Status})"
                );

                return Ok(new { message = "Usuario actualizado exitosamente." });

            } catch (MySqlException ex) {
                return StatusCode(500, new { error = ex.Message });

            } catch (Exception ex) {
                return StatusCode(500, new { message = "Error inesperado", error = ex.Message });
            }
        }
 // POST: api/users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> Postusers([FromBody] vw_user user) {
            try {
                // Encriptar la contraseña antes de guardarla
                string hashedPassword = BcryptNet.HashPassword(user.Password);

                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL sp_insert_user({user.Name}, {user.Email}, {user.Role}, {hashedPassword})"
                );
                return Ok(new { message = "Usuario insertado exitosamente." });
            } catch (MySqlException ex) {
                return StatusCode(500,new { error = ex.Message });
            } catch (Exception ex) {
                return StatusCode(500,new { message = "Error inesperado",error = ex.Message });
            }
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deleteusers(int id) {
            try {
                var users_delete = await _context.Database
                    .ExecuteSqlInterpolatedAsync($"CALL sp_delete_user({id})");

                return Ok(users_delete);
            } catch (Exception ex) {
                return StatusCode(500,new { message = "Error inesperado",error = ex.Message });
            }
        }

    }
}
