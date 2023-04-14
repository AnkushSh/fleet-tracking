using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using fleet_tracking.Data;
using fleet_tracking.Models;
using Microsoft.AspNetCore.Authorization;

namespace fleet_tracking.Controllers
{
    [Authorize(Roles = "Admin, SecurityPersonnel")]
    public class SecurityTeamController : Controller
    {
        private readonly QweDbContext _context;

        public SecurityTeamController(QweDbContext context)
        {
            _context = context;
        }

        // GET: SecurityTeam
        public async Task<IActionResult> Index()
        {
            ViewBag.Vehicles = _context.Vehicles.ToList();
            return View(await _context.SecurityTeams.ToListAsync());
        }

        // GET: SecurityTeam/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SecurityTeams == null)
            {
                return NotFound();
            }

            var securityTeam = await _context.SecurityTeams
                .Include(s => s.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (securityTeam == null)
            {
                return NotFound();
            }

            return View(securityTeam);
        }

        // GET: SecurityTeam/Create
        public IActionResult Create()
        {
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Id");
            return View();
        }

        // POST: SecurityTeam/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TeamName,VehicleId")] SecurityTeam securityTeam)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.SelectMany(x => x.Value.Errors.Select(e => e.ErrorMessage));
                foreach (var err in error)
                {
                    Console.WriteLine($"ModelState Error: {err}");
                }
            }
            
            ModelState.Clear(); // Clear ModelState
            
            if (ModelState.IsValid)
            {
                _context.Add(securityTeam);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Id", securityTeam.VehicleId);
            return View(securityTeam);
        }

        // GET: SecurityTeam/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SecurityTeams == null)
            {
                return NotFound();
            }

            var securityTeam = await _context.SecurityTeams.FindAsync(id);
            if (securityTeam == null)
            {
                return NotFound();
            }
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Id", securityTeam.VehicleId);
            return View(securityTeam);
        }

        // POST: SecurityTeam/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TeamName,VehicleId")] SecurityTeam securityTeam)
        {
            if (id != securityTeam.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(securityTeam);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SecurityTeamExists(securityTeam.Id))
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
            ViewData["VehicleId"] = new SelectList(_context.Vehicles, "Id", "Id", securityTeam.VehicleId);
            return View(securityTeam);
        }

        // GET: SecurityTeam/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SecurityTeams == null)
            {
                return NotFound();
            }

            var securityTeam = await _context.SecurityTeams
                .Include(s => s.Vehicle)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (securityTeam == null)
            {
                return NotFound();
            }

            return View(securityTeam);
        }

        // POST: SecurityTeam/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SecurityTeams == null)
            {
                return Problem("Entity set 'QweDbContext.SecurityTeams'  is null.");
            }
            var securityTeam = await _context.SecurityTeams.FindAsync(id);
            if (securityTeam != null)
            {
                _context.SecurityTeams.Remove(securityTeam);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignVehicle(int id, int vehicleId)
        {
            var securityTeam = await _context.SecurityTeams.FindAsync(id);
            if (securityTeam == null)
            {
                return NotFound();
            }

            securityTeam.VehicleId = vehicleId;
            _context.Update(securityTeam);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DismissVehicle(int id)
        {
            var securityTeam = await _context.SecurityTeams.FindAsync(id);
            if (securityTeam == null)
            {
                return NotFound();
            }

            securityTeam.VehicleId = null; // Set to 0 to indicate no assigned vehicle
            _context.Update(securityTeam);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        private bool SecurityTeamExists(int id)
        {
          return (_context.SecurityTeams?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
