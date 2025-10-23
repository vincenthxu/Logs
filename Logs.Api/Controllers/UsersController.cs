using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Logs.Api.Data;
using Logs.Api.Models;
using Microsoft.AspNetCore.Authorization;

namespace Logs.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly LogsContext _context;
        private readonly IdentityContext _auth;

        public UsersController(LogsContext context, IdentityContext auth)
        {
            _context = context;
            _auth = auth;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // GET: api/Users/byEmail/{email}
        [HttpGet("byEmail/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<ActionResult<User>> GetUserByEmail(string email)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> PutUser(Guid id, User user)
        {
            // if user has invalid date of birth, return bad request
            if (user.HasInvalidDateOfBirth)
            {
                return BadRequest("Date of birth has not yet occurred!");
            }

            user.Id = id;

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            var result = _auth.Users.Where(u => u.Email == user.Email).ToList();
            if (result.Count != 1 || _context.Users.SingleOrDefault(u => u.Email == user.Email) != null)
            {
                return Unauthorized();
            }

            // if user has invalid date of birth, return bad request
            if (user.HasInvalidDateOfBirth)
            {
                return BadRequest("Date of birth has not yet occurred!");
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // when a User is deleted, also delete all of their associated Entries
            var entries = _context.Entries.Where(e => e.UserId == user.Id);
            _context.Entries.RemoveRange(entries.ToArray());

            // when a User is deleted, also unregister them
            _auth.Users.Where(u => u.Email == user.Email).ExecuteDelete();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
