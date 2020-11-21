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
using System.IO;

namespace web.Controllers
{
    // Require login for everything	
    [Authorize]
    public class PlantsController : Controller
    {
        private readonly PileaContext _context;

        // Object for fetching user info.	
        private readonly UserManager<User> _usermanager;

        public PlantsController(PileaContext context, UserManager<User> userManager)
        {
            _context = context;
            _usermanager = userManager;
        }

        // GET: Plants
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {

//             Get current user.
            var currentUser = await _usermanager.GetUserAsync(User);
            
            // define sort parameters
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DaysBetweenWateringParm"] = sortOrder == "DaysBetweenWatering" ? "DaysBetweenWatering_desc" : "DaysBetweenWatering";
            ViewData["LastWateredDateParm"] = sortOrder == "LastWateredDate" ? "LastWateredDate_desc" : "LastWateredDate";
            ViewData["NextWateredDateParm"] = sortOrder == "NextWateredDate" ? "NextWateredDate_desc" : "NextWateredDate";
            ViewData["CategoryParm"] = sortOrder == "Category" ? "Category_desc" : "Category";
            ViewData["LocationParm"] = sortOrder == "Location" ? "Location_desc" : "Location";

             // define search parameters
            ViewData["CurrentFilter"] = searchString;


            // Query
            var plants = from p in _context.Plants.Where(p => p.User == currentUser).Include(p => p.Category).Include(p => p.Location) select p;


            // search
            if (!String.IsNullOrEmpty(searchString))
            {
                plants = plants.Where(p => p.Name.Contains(searchString)
                                        || p.Description.Contains(searchString)
                                        || p.Note.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    plants = plants.OrderByDescending(p => p.Name);
                    break;
                case "DaysBetweenWatering":
                    plants = plants.OrderBy(p => p.DaysBetweenWatering);
                    break;
                case "DaysBetweenWatering_desc":
                    plants = plants.OrderByDescending(p => p.DaysBetweenWatering);
                    break;
                case "LastWateredDate":
                    plants = plants.OrderBy(p => p.LastWateredDate);
                    break;
                case "LastWateredDate_desc":
                    plants = plants.OrderByDescending(p => p.LastWateredDate);
                    break;
                case "NextWateredDateParm":
                    plants = plants.OrderBy(p => p.NextWateredDate);
                    break;
                case "NextWateredDateParm_desc":
                    plants = plants.OrderByDescending(p => p.NextWateredDate);
                    break;
                case "Category":
                    plants = plants.OrderBy(p => p.Category);
                    break;
                case "Category_desc":
                    plants = plants.OrderByDescending(p => p.Category);
                    break;
                case "Location":
                    plants = plants.OrderBy(p => p.Location);
                    break;
                case "Location_desc":
                    plants = plants.OrderByDescending(p => p.Location);
                    break;
                default:
                    plants = plants.OrderBy(p => p.Name);
                    break;
            }
            return View(await plants.AsNoTracking().ToListAsync());

        
        }

        // GET: Plants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plant = await _context.Plants
                .Include(p => p.Category)
                .Include(p => p.Location)
                .FirstOrDefaultAsync(m => m.PlantID == id);
            if (plant == null)
            {
                return NotFound();
            }

            return View(plant);
        }

        // GET: Plants/Create
        public IActionResult Create()
        {
            var currentUserID = _usermanager.GetUserId(User);
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "PlantCategory");
            ViewData["LocationID"] = new SelectList(_context.Locations.Where(l => l.User.Id == currentUserID), "LocationID", "Name");
            return View();
        }

        // POST: Plants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlantID,Name,Description,Note,image,DaysBetweenWatering,LastWateredDate,NextWateredDate,CategoryID,LocationID")] Plant plant)
        {

            // Get ApplicationUser object (plant owner)	
            var currentUser = await _usermanager.GetUserAsync(User);
            var pileaContext = _context.Plants.Where(p => p.User == currentUser).Include(p => p.Category).Include(p => p.Location);
            if (ModelState.IsValid)
            {
                plant.User = currentUser;
                Console.WriteLine("to je image "+Request.Form.Files[0]);
                plant.image = UploadImage(Request);
                
                _context.Add(plant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryID", plant.CategoryID);
            ViewData["LocationID"] = new SelectList(_context.Locations.Where(l => l.User == currentUser), "LocationID", "LocationID", plant.LocationID);
            return View(plant);
        }



        // GET: Plants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
             // Get ApplicationUser object (plant owner)	
            var currentUser = await _usermanager.GetUserAsync(User);
            if (id == null)
            {
                return NotFound();
            }

            var plant = await _context.Plants.FindAsync(id);
            if (plant == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "PlantCategory", plant.CategoryID);
            ViewData["LocationID"] = new SelectList(_context.Locations.Where(l => l.User == currentUser), "LocationID", "Name", plant.LocationID);
            return View(plant);
        }

        // POST: Plants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlantID,Name,Description,Note,image,DaysBetweenWatering,LastWateredDate,NextWateredDate,CategoryID,LocationID")] Plant plant)
        {
            if (id != plant.PlantID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlantExists(plant.PlantID))
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
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryID", plant.CategoryID);
            ViewData["LocationID"] = new SelectList(_context.Locations, "LocationID", "LocationID", plant.LocationID);
            return View(plant);
        }

        // GET: Plants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plant = await _context.Plants
                .Include(p => p.Category)
                .Include(p => p.Location)
                .FirstOrDefaultAsync(m => m.PlantID == id);
            if (plant == null)
            {
                return NotFound();
            }

            return View(plant);
        }

        // POST: Plants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plant = await _context.Plants.FindAsync(id);
            _context.Plants.Remove(plant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlantExists(int id)
        {
            return _context.Plants.Any(e => e.PlantID == id);
        }



        [HttpPost]
        public byte[] UploadImage(Microsoft.AspNetCore.Http.HttpRequest request)
        {
            foreach(var file in request.Form.Files)
            {
                Plant img = new Plant();
                var ImageTitle = file.FileName;

                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                img.image = ms.ToArray();

                ms.Close();
                ms.Dispose();
                Console.Write("IM WRIGINT IMAGE "+file);
                ViewBag.Message = "Image(s) stored in database!";
                return img.image;
            }
            return null;
            
            
            //return View("Index");
        }

    }
}
