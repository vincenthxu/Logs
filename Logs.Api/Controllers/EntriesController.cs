using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Logs.Api.Data;
using Logs.Api.Models;

namespace Logs.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntriesController : ControllerBase
    {
        private readonly LogsContext _context;

        public EntriesController(LogsContext context)
        {
            _context = context;
        }

        // GET: api/Entries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Entry>>> GetEntries()
        {
            return await _context.Entries.ToListAsync();
        }

        // GET: api/Entries/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Entry>> GetEntry(int id)
        {
            var entry = await _context.Entries.FindAsync(id);

            if (entry == null)
            {
                return NotFound();
            }

            return entry;
        }

        // GET: api/Entries/user/{id}
        [HttpGet("user/{id}")]
        public async Task<ActionResult<IEnumerable<Entry>>> GetEntriesByUserId(Guid id)
        {
            var entries = await _context.Entries
                .Where(e => e.UserId == id)
                .ToListAsync();
            return entries;
        }

        // PUT: api/Entries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutEntry(int id, Entry entry)
        {
            if (id != entry.Id)
            {
                return BadRequest();
            }

            _context.Entry(entry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntryExists(id))
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

        // POST: api/Entries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Entry>> PostEntry(Entry entry)
        {
            _context.Entries.Add(entry);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEntry", new { id = entry.Id }, entry);
        }

        // DELETE: api/Entries/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEntry(int id)
        {
            var entry = await _context.Entries.FindAsync(id);
            if (entry == null)
            {
                return NotFound();
            }

            _context.Entries.Remove(entry);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EntryExists(int id)
        {
            return _context.Entries.Any(e => e.Id == id);
        }
    }
}
