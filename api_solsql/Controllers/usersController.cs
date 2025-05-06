using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_solsql.Context;
using api_solsql.Models;
using Microsoft.Data.SqlClient;
using MySqlConnector;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace api_solsql.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usersController : ControllerBase
    {
        private readonly ContextDB _context;

        public usersController(ContextDB context)
        {
            _context = context;
        }

        // GET: api/users/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<users>>> Getusers() {
            try {
                var usersList = await _context.Users
                    .FromSqlRaw("CALL sp_get_all_users()")
                    .ToListAsync();
                return Ok(usersList);
            } catch (Exception ex) {
                return StatusCode(500,new { message = "Error al obtener los usuarios",error = ex.Message });
            }
        }
          // GET: api/users/status/{status}
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<users>>> GetUsersByStatus(int status) {
            try {
                var usersList = new List<users>();

                if (status == 1) {
                    usersList = await _context.Users
                        .FromSqlRaw("CALL sp_get_all_users_active()")
                        .ToListAsync();
                } else if (status == 0) {
                    usersList = await _context.Users
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
         // GET: api/users/email
        [HttpGet("{email}")]
        public async Task<ActionResult<users>> Getusers(String email)
        {
            try {
                var users = await _context.Users
                .FromSqlInterpolated($"CALL sp_get_user_by_email({email})")
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
        public async Task<IActionResult> Putusers(int id, users user)
        {
            try {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL sp_update_user({id}, {user.Name_user}, {user.Email}, {user.User_type}, {user.Password}, {user.Is_active})"
                );
                return Ok(new { message = "Usuario actualizado exitosamente." });
            } catch (MySqlException ex) {
                return StatusCode(500,new { error = ex.Message });
            } catch (Exception ex) {
                return StatusCode(500,new { message = "Error inesperado",error = ex.Message });
            }
        }
        // POST: api/users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> Postusers([FromBody] users user) {
            try {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL sp_insert_user({user.Name_user}, {user.Email}, {user.User_type}, {user.Password})"
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
