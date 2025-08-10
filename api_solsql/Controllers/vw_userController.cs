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
        public async Task<ActionResult<IEnumerable<vw_user>>> Getusers()
        {
            try
            {
                var usersList = await _context.VW_users
                    .FromSqlRaw("CALL sp_get_all_users()")
                    .ToListAsync();
                return Ok(usersList);
            }
            catch (MySqlException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Unexpected error", error = ex.Message });
            }
        }
        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<vw_user>> Getusers(int id)
        {
            try
            {
                var users = await _context.VW_users
                    .FromSqlInterpolated($"CALL sp_get_user_by_id({id})")
                    .ToListAsync();

                if (users == null || users.Count == 0)
                {
                    return NotFound(new { message = "ERROR: User not found" });
                }

                return Ok(users);
            }
            catch (MySqlException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Unexpected error", error = ex.Message });
            }
        }
        // GET: api/users/status/{status}
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<vw_user>>> GetUsersByStatus(int status)
        {
            try
            {
                var usersList = new List<vw_user>();

                if (status == 1)
                {
                    usersList = await _context.VW_users
                        .FromSqlRaw("CALL sp_get_all_users_active()")
                        .ToListAsync();
                }
                else if (status == 0)
                {
                    usersList = await _context.VW_users
                        .FromSqlRaw("CALL sp_get_all_users_desactive()")
                        .ToListAsync();
                }
                else
                {
                    return BadRequest(new { message = "ERROR: The State must be active (1) or inactive (0)" });
                }

                return Ok(usersList);
            }
            catch (MySqlException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Unexpected error", error = ex.Message });
            }
        }
        // PUT: api/users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putusers(int id, vw_user user)
        {
            try
            {
                // Verifica si la contraseña ya está hasheada o si es una nueva
                if (!user.Password.StartsWith("$2a$"))
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                }

                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL sp_update_user({id}, {user.Name}, {user.Email}, {user.Role}, {user.Password}, {user.Status})"
                );

                return Ok(new { message = "User updated successfully." });

            }
            catch (MySqlException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Unexpected error", error = ex.Message });
            }
        }
        // POST: api/users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> Postusers([FromBody] vw_user user)
        {
            try
            {
                // Encriptar la contraseña antes de guardarla
                string hashedPassword = BcryptNet.HashPassword(user.Password);

                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL sp_insert_user({user.Name}, {user.Email}, {user.Role}, {hashedPassword})"
                );
                return Ok(new { message = "User inserted successfully." });
            }
            catch (MySqlException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Unexpected error", error = ex.Message });
            }
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deleteusers(int id)
        {
            try
            {
                var users_delete = await _context.Database
                    .ExecuteSqlInterpolatedAsync($"CALL sp_delete_user({id})");
                return Ok(users_delete);
            }
            catch (MySqlException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Unexpected error", error = ex.Message });
            }
        }
    }
}
