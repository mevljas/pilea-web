using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;
using web.Filters;

namespace web.Controllers_Api
{
    [Route("api/Friendship")]
    [ApiController]
    [ApiKeyAuth]
    public class FriendshipsApiController : ControllerBase
    {
        private readonly PileaContext _context;

        public FriendshipsApiController(PileaContext context)
        {
            _context = context;
        }

        // GET: api/FriendshipsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Friendship>>> GetFriendships()
        {
            return await _context.Friendships.ToListAsync();
        }

        // GET: api/FriendshipsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Friendship>> GetFriendship(string id)
        {
            var friendship = await _context.Friendships.FindAsync(id);

            if (friendship == null)
            {
                return NotFound();
            }

            return friendship;
        }

        // PUT: api/FriendshipsApi/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFriendship(string id, Friendship friendship)
        {
            if (id != friendship.UserId)
            {
                return BadRequest();
            }

            _context.Entry(friendship).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FriendshipExists(id))
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

        // POST: api/FriendshipsApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Friendship>> PostFriendship(Friendship friendship)
        {
            _context.Friendships.Add(friendship);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FriendshipExists(friendship.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetFriendship", new { id = friendship.UserId }, friendship);
        }

        // DELETE: api/FriendshipsApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Friendship>> DeleteFriendship(string id)
        {
            var friendship = await _context.Friendships.FindAsync(id);
            if (friendship == null)
            {
                return NotFound();
            }

            _context.Friendships.Remove(friendship);
            await _context.SaveChangesAsync();

            return friendship;
        }

        private bool FriendshipExists(string id)
        {
            return _context.Friendships.Any(e => e.UserId == id);
        }
    }
}
