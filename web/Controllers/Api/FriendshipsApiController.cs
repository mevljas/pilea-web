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
using Microsoft.AspNetCore.Authorization;

namespace web.Controllers_Api
{
    [Route("api/Friendship")]
    [ApiController]
    [ApiKeyAuth]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class FriendshipsApiController : ControllerBase
    {
        private readonly PileaContext _context;

        public FriendshipsApiController(PileaContext context)
        {
            _context = context;
        }

        // GET: api/FriendshipsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Friendship>>> GetFriendships([FromQuery(Name = "userId")] string userId)
        {
            // return await _context.Friendships.ToListAsync();
            return await _context.Friendships.Where(p => p.User.Id == userId).ToListAsync();
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
        public async Task<ActionResult<Friendship>> PostFriendship(Friendship friendship, [FromQuery(Name = "userId")] string userId, [FromQuery(Name = "userFriendId")] string userFriendId)
        {
            var user = await _context.Users.FindAsync(userId);
            var friend = await _context.Users.FindAsync(userFriendId);
            friendship.User = user;
            friendship.UserFriend = friend;
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

            // Prevent looping dependencies
            friendship.User = null;
            friendship.UserFriend = null;

            return CreatedAtAction("GetFriendship", new { id = friendship.UserId }, friendship);
        }

        // DELETE: api/FriendshipsApi/5
        [HttpDelete("{userId}/{userFriendId}")]
        public async Task<ActionResult<Friendship>> DeleteFriendship(string userId, string userFriendId)
        {
            // var friendship = await _context.Friendships.FindAsync(id);
            var friendship = await _context.Friendships.FindAsync(userId, userFriendId);
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
