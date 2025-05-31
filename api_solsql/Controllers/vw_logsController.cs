using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_solsql.Models;
namespace api_solsql.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class vw_logsController : ControllerBase
    {
        private readonly Context.ContextDB _context;

        public vw_logsController(Context.ContextDB context)
        {
            _context = context;
        }

        // GET: api/vw_logs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<vw_logs>>> GetLogs()
        {
            try
            {
                var logsList = await _context.vw_log
                    .FromSqlInterpolated($"CALL sp_get_logs()")
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
