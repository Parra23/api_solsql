using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_solsql.Context;

namespace api_solsql.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class vw_totalcomments_date : ControllerBase
    {
        private readonly ContextDB _context;

        public vw_totalcomments_date(ContextDB context)
        {
            _context = context;
        }

        // GET: api/vw_totalcomments_date
        [HttpGet]
        public async Task<ActionResult<IEnumerable<vw_totalcomments_date>>> GetTotalCommentsDate()
        {
            try
            {
                var totalComments = await _context.vw_totalcomments_dates
                    .FromSqlInterpolated($"CALL sp_totalcomments_date()")
                    .ToListAsync();

                return Ok(totalComments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Unexpected error {ex.Message}");
            }
        }
    }
}
