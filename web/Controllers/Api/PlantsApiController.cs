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
using web;

namespace web.Controllers_Api
{
    [Route("api/Plant")]
    [ApiController]
    [ApiKeyAuth]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PlantsApiController : ControllerBase
    {
        private readonly PileaContext _context;

        public PlantsApiController(PileaContext context)
        {
            _context = context;
        }

        // GET: api/PlantsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plant>>> GetPlants([FromQuery(Name = "userId")] string userId)
        {
            // return await _context.Plants.ToListAsync();
            return await _context.Plants.Where(p => p.User.Id == userId).ToListAsync();
        }

        // GET: api/PlantsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Plant>> GetPlant(int id)
        {
            var plant = await _context.Plants.FindAsync(id);

            if (plant == null)
            {
                return NotFound();
            }

            return plant;
        }

        // PUT: api/PlantsApi/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlant(int id, Plant plant)
        {
            if (id != plant.PlantID)
            {
                return BadRequest();
            }

            _context.Entry(plant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlantExists(id))
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

        // POST: api/PlantsApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Plant>> PostPlant(Plant plant, [FromQuery(Name = "userId")] string userId)
        {
            _context.Plants.Add(plant);
            var user = await _context.Users.FindAsync(userId);
            plant.User = user;
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlant", new { id = plant.PlantID }, plant);
        }

        // DELETE: api/PlantsApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Plant>> DeletePlant(int id)
        {
            var plant = await _context.Plants.FindAsync(id);
            if (plant == null)
            {
                return NotFound();
            }

            _context.Plants.Remove(plant);
            await _context.SaveChangesAsync();

            return plant;
        }

        private bool PlantExists(int id)
        {
            return _context.Plants.Any(e => e.PlantID == id);
        }
    }
}
