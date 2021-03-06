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
    [Route("api/location")]
    [ApiController]
    [ApiKeyAuth]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class LocationsApiController : ControllerBase
    {
        private readonly PileaContext _context;

        public LocationsApiController(PileaContext context)
        {
            _context = context;
        }

        // GET: api/LocationsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocations([FromQuery(Name = "userId")] string userId)
        {
            // return await _context.Locations.ToListAsync();
            return await _context.Locations.Where(p => p.User.Id == userId).ToListAsync();
        }

        // GET: api/LocationsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Location>> GetLocation(int id)
        {
            var location = await _context.Locations.FindAsync(id);

            if (location == null)
            {
                return NotFound();
            }

            return location;
        }

        // PUT: api/LocationsApi/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocation(int id, Location location)
        {
            if (id != location.LocationID)
            {
                return BadRequest();
            }

            _context.Entry(location).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(id))
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

        // POST: api/LocationsApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Location>> PostLocation(Location location, [FromQuery(Name = "userId")] string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            location.User = user;
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLocation", new { id = location.LocationID }, location);
        }

        // DELETE: api/LocationsApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Location>> DeleteLocation(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();

            return location;
        }

        private bool LocationExists(int id)
        {
            return _context.Locations.Any(e => e.LocationID == id);
        }
    }
}
