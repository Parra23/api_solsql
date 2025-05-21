using api_solsql.Context;
using api_solsql.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace api_solsql.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginRequestController : ControllerBase
    {
        private readonly ContextDB _context;

        public LoginRequestController(ContextDB context)
        {
            _context = context;
        }

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<ActionResult<LoginRequest>> Login([FromBody] LoginRequest request)
        {
            try
            {
                var users = await _context.LoginRequests
                    .FromSqlInterpolated($"CALL sp_login({request.Email}, {request.Password})")
                    .ToListAsync();

                if (users.Count == 0)
                    return Unauthorized(new { message = "Credenciales inválidas" });

                return Ok(users.First());
            }
            catch (MySqlException ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error inesperado", error = ex.Message });
            }
        }
    }
}
