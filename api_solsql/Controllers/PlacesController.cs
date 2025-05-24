using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_solsql.Context;
using api_solsql.Models;

namespace api_solsql.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlacesController : ControllerBase
    {
        private readonly ContextDB _context;

        public PlacesController(ContextDB context)
        {
            _context = context;
        }

        // GET: api/Places
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Places>>> Getplaces()
        {
            return await _context.places.ToListAsync();
        }

        // GET: api/Places/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Places>> GetPlaces(int id)
        {
            var places = await _context.places.FindAsync(id);

            if (places == null)
            {
                return NotFound();
            }

            return places;
        }

        // PUT: api/Places/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlaces(int id, Places places)
        {
            if (id != places.place_id)
            {
                return BadRequest();
            }

            _context.Entry(places).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlacesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Places
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Places>> PostPlaces(Places places)
        {
            _context.places.Add(places);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlaces", new { id = places.place_id }, places);
        }

        // DELETE: api/Places/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlaces(int id)
        {
            var places = await _context.places.FindAsync(id);
            if (places == null)
            {
                return NotFound();
            }

            _context.places.Remove(places);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlacesExists(int id)
        {
            return _context.places.Any(e => e.place_id == id);
        }
    }
}
