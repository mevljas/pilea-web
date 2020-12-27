using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;
using Microsoft.AspNetCore.Authorization;	
using Microsoft.AspNetCore.Identity;

namespace web.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly PileaContext _context;

        // Object for fetching user info.	
        private readonly UserManager<User> _usermanager;

        public CategoriesController(PileaContext context, UserManager<User> userManager)
        {
            _context = context;
            _usermanager = userManager;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var currentUser = await _usermanager.GetUserAsync(User);
            return View(await _context.Categories.Where(p => p.User == currentUser).ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var currentUser = await _usermanager.GetUserAsync(User);
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(p => p.Plants)	
                .Where(p => p.User == currentUser)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.CategoryID == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("CategoryID,PlantCategory")] Category category)
        {
            // Get ApplicationUser object (plant owner)	
            var currentUser = await _usermanager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                category.User = currentUser;
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            var currentUser = await _usermanager.GetUserAsync(User);
            if (category == null || category.User != currentUser)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryID,PlantCategory")] Category category)
        {
            var currentUser = await _usermanager.GetUserAsync(User);
            if (id != category.CategoryID || category.User != currentUser)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentUser = await _usermanager.GetUserAsync(User);

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryID == id && m.User == currentUser);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            var currentUser = await _usermanager.GetUserAsync(User);
            if (category.User != currentUser)
            {
                return NotFound();
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryID == id);
        }
    }
}
