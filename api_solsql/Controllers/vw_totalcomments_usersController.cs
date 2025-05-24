using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_solsql.Context;
using api_solsql.Models;

namespace api_solsql.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class vw_totalcomments_usersController : ControllerBase
    {
        private readonly ContextDB _context;

        public vw_totalcomments_usersController(ContextDB context)
        {
            _context = context;
        }

        // GET: api/vw_totalcomments_users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<vw_totalcomments_users>>> GetTotalCommentsUsers()
        {
            try
            {
                var totalCommentsUsers = await _context.vw_totalcomments_users
                    .FromSqlInterpolated($"CALL sp_totalcomments_users()")
                    .ToListAsync();

                return Ok(totalCommentsUsers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unexpected error {ex.Message}");
            }
        }

    }
}
