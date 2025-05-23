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
    public class PhotosController : ControllerBase
    {
        private readonly ContextDB _context;

        public PhotosController(ContextDB context)
        {
            _context = context;
        }

        // GET: api/Photos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Photos>>> Getphotos()
        {
            return await _context.photos.ToListAsync();
        }

        // GET: api/Photos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Photos>> GetPhotos(int id)
        {
            var photos = await _context.photos.FindAsync(id);

            if (photos == null)
            {
                return NotFound();
            }

            return photos;
        }

        // PUT: api/Photos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhotos(int id, Photos photos)
        {
            if (id != photos.photo_id)
            {
                return BadRequest();
            }

            _context.Entry(photos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhotosExists(id))
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

        // POST: api/Photos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Photos>> PostPhotos(Photos photos)
        {
            _context.photos.Add(photos);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPhotos", new { id = photos.photo_id }, photos);
        }

        // DELETE: api/Photos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhotos(int id)
        {
            var photos = await _context.photos.FindAsync(id);
            if (photos == null)
            {
                return NotFound();
            }

            _context.photos.Remove(photos);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PhotosExists(int id)
        {
            return _context.photos.Any(e => e.photo_id == id);
        }
    }
}
