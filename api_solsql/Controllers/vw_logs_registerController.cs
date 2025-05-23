using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_solsql.Context;
using api_solsql.Models;
using MySqlConnector;

namespace api_solsql.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class vw_logs_registerController : ControllerBase
    {
        private readonly ContextDB _context;

        public vw_logs_registerController(ContextDB context)
        {
            _context = context;
        }

        // GET: api/vw_logs_register/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<vw_logs_register>>> GetLogs()
        {
            try
            {
                var logsList = await _context.vw_logs_registers
                    .FromSqlRaw("CALL sp_get_logs_register()")
                    .ToListAsync();
                return Ok(logsList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Unexpected error", error = ex.Message });
            }
        }
    }
}
