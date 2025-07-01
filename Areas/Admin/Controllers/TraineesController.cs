using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.Data;
using BusinessLogic.Models;

namespace BusinessLogic.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TraineesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TraineesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Trainees
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Trainee.Include(t => t.Batch).Include(t => t.College);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Trainees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainee = await _context.Trainee
                .Include(t => t.Batch)
                .Include(t => t.College)
                .FirstOrDefaultAsync(m => m.TraineeId == id);
            if (trainee == null)
            {
                return NotFound();
            }

            return View(trainee);
        }

        // GET: Admin/Trainees/Create
        public IActionResult Create()
        {
            ViewData["BatchId"] = new SelectList(_context.Set<Batch>(), "BatchId", "BatchId");
            ViewData["CollegeId"] = new SelectList(_context.Set<College>(), "CollegeId", "CollegeId");
            return View();
        }

        // POST: Admin/Trainees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TraineeId,BatchId,CollegeId,TraineeName,TraineeLocation,Email,Phone,Discontinue")] Trainee trainee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trainee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BatchId"] = new SelectList(_context.Set<Batch>(), "BatchId", "BatchId", trainee.BatchId);
            ViewData["CollegeId"] = new SelectList(_context.Set<College>(), "CollegeId", "CollegeId", trainee.CollegeId);
            return View(trainee);
        }

        // GET: Admin/Trainees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainee = await _context.Trainee.FindAsync(id);
            if (trainee == null)
            {
                return NotFound();
            }
            ViewData["BatchId"] = new SelectList(_context.Set<Batch>(), "BatchId", "BatchId", trainee.BatchId);
            ViewData["CollegeId"] = new SelectList(_context.Set<College>(), "CollegeId", "CollegeId", trainee.CollegeId);
            return View(trainee);
        }

        // POST: Admin/Trainees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TraineeId,BatchId,CollegeId,TraineeName,TraineeLocation,Email,Phone,Discontinue")] Trainee trainee)
        {
            if (id != trainee.TraineeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TraineeExists(trainee.TraineeId))
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
            ViewData["BatchId"] = new SelectList(_context.Set<Batch>(), "BatchId", "BatchId", trainee.BatchId);
            ViewData["CollegeId"] = new SelectList(_context.Set<College>(), "CollegeId", "CollegeId", trainee.CollegeId);
            return View(trainee);
        }

        // GET: Admin/Trainees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainee = await _context.Trainee
                .Include(t => t.Batch)
                .Include(t => t.College)
                .FirstOrDefaultAsync(m => m.TraineeId == id);
            if (trainee == null)
            {
                return NotFound();
            }

            return View(trainee);
        }

        // POST: Admin/Trainees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainee = await _context.Trainee.FindAsync(id);
            if (trainee != null)
            {
                _context.Trainee.Remove(trainee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TraineeExists(int id)
        {
            return _context.Trainee.Any(e => e.TraineeId == id);
        }
    }
}
