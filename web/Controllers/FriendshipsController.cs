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
using Microsoft.AspNetCore.Authorization;	
using Microsoft.AspNetCore.Identity;

namespace web.Controllers
{
    [Authorize]
    public class FriendshipsController : Controller
    {
        private readonly PileaContext _context;

        // Object for fetching user info.	
        private readonly UserManager<User> _usermanager;

        public FriendshipsController(PileaContext context, UserManager<User> userManager)
        {
            _context = context;
            _usermanager = userManager;
        }

        // GET: Friendships
        public async Task<IActionResult> Index()
        {
            var currentUser = await _usermanager.GetUserAsync(User);
            var pileaContext = _context.Friendships.Where(p => p.User == currentUser).Include(f => f.User).Include(f => f.UserFriend);
            return View(await pileaContext.ToListAsync());
        }

        // GET: Friendships/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friendship = await _context.Friendships
                .Include(f => f.User)
                .Include(f => f.UserFriend)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (friendship == null)
            {
                return NotFound();
            }

            return View(friendship);
        }

        // GET: Friendships/Create
        public IActionResult Create()
        {
            var currentUserID = _usermanager.GetUserId(User);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["UserFriendId"] = new SelectList(_context.Users.Where(l => l.Id != currentUserID), "Id", "Email");
            return View();
        }

        // POST: Friendships/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,UserFriendId")] Friendship friendship)
        {
            // Get ApplicationUser object (plant owner)	
            var currentUser = await _usermanager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                friendship.UserId = currentUser.Id;
                _context.Add(friendship);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", friendship.UserId);
            ViewData["UserFriendId"] = new SelectList(_context.Users, "Id", "Id", friendship.UserFriendId);
            return View(friendship);
        }

        // GET: Friendships/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friendship = await _context.Friendships.FindAsync(id);
            if (friendship == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", friendship.UserId);
            ViewData["UserFriendId"] = new SelectList(_context.Users, "Id", "Id", friendship.UserFriendId);
            return View(friendship);
        }

        // POST: Friendships/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserId,UserFriendId")] Friendship friendship)
        {
            if (id != friendship.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(friendship);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FriendshipExists(friendship.UserId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", friendship.UserId);
            ViewData["UserFriendId"] = new SelectList(_context.Users, "Id", "Id", friendship.UserFriendId);
            return View(friendship);
        }

        // GET: Friendships/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var friendship = await _context.Friendships
                .Include(f => f.User)
                .Include(f => f.UserFriend)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (friendship == null)
            {
                return NotFound();
            }

            return View(friendship);
        }

        // POST: Friendships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var friendship = await _context.Friendships.FindAsync(id);
            _context.Friendships.Remove(friendship);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FriendshipExists(string id)
        {
            return _context.Friendships.Any(e => e.UserId == id);
        }
    }
}
