using api_solsql.Context;
using api_solsql.Models;
using Microsoft.AspNetCore.Http;
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
                    return Unauthorized(new { message = "Usuario no encontrado o rol inválido" });

                var user = users.First();

                // 2) Verificar la contraseña (texto plano vs. hash)
                bool isPasswordValid = BcryptNet.Verify(request.Password, user.Password);
                if (!isPasswordValid)
                    return Unauthorized(new { message = "Credenciales inválidas" });

                // 3) Limpiar el hash antes de devolver el objeto
                user.Password = null;

                // 4) (Opcional) Generar un token JWT o algún dato adicional aquí

                // 5) Retornar Ok con el usuario sin el campo Password
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error inesperado", error = ex.Message });
            }
        }
    }
}