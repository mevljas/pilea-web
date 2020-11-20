using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;

namespace web.Controllers
{
    public class LocalUsersController : Controller
    {
        private readonly PileaContext _context;

        public LocalUsersController(PileaContext context)
        {
            _context = context;
        }

        // GET: LocalUsers
        public async Task<IActionResult> Index()
        {
            return View(await _context.LocalUser.ToListAsync());
        }

        // GET: LocalUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var localUser = await _context.LocalUser
                .FirstOrDefaultAsync(m => m.LocalUserID == id);
            if (localUser == null)
            {
                return NotFound();
            }

            return View(localUser);
        }

        // GET: LocalUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LocalUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LocalUserID,Username,Nickname,Password")] LocalUser localUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(localUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(localUser);
        }

        // GET: LocalUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var localUser = await _context.LocalUser.FindAsync(id);
            if (localUser == null)
            {
                return NotFound();
            }
            return View(localUser);
        }

        // POST: LocalUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LocalUserID,Username,Nickname,Password")] LocalUser localUser)
        {
            if (id != localUser.LocalUserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(localUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocalUserExists(localUser.LocalUserID))
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
            return View(localUser);
        }

        // GET: LocalUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var localUser = await _context.LocalUser
                .FirstOrDefaultAsync(m => m.LocalUserID == id);
            if (localUser == null)
            {
                return NotFound();
            }

            return View(localUser);
        }

        // POST: LocalUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var localUser = await _context.LocalUser.FindAsync(id);
            _context.LocalUser.Remove(localUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocalUserExists(int id)
        {
            return _context.LocalUser.Any(e => e.LocalUserID == id);
        }
    }
}
