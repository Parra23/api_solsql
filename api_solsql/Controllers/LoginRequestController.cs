using api_solsql.Context;
using api_solsql.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using BcryptNet = BCrypt.Net.BCrypt;

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
                // 1) Traer el usuario por email + rol
                var users = await _context.LoginRequests
                    .FromSqlInterpolated($"CALL sp_login({request.Email}, {request.Role})")
                    .ToListAsync();

                if (users.Count == 0)
                    return Unauthorized(new { message = "User not found" });
                var user = users.First();
                // 1.1) Verificar si el rol es correcto
                if (user.Role != request.Role)
                    return Unauthorized(new { message = "Invalid role" });
                // 2) Verificar la contraseña (texto plano vs. hash)
                bool isPasswordValid = BcryptNet.Verify(request.Password, user.Password);
                if (!isPasswordValid)
                    return Unauthorized(new { message = "Invalid credentials" });
                // 3) Limpiar el hash antes de devolver el objeto
                user.Password = null;
                // 4) (Opcional) Generar un token JWT o algún dato adicional aquí
                // 5) Retornar Ok con el usuario sin el campo Password
                return Ok(user);
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