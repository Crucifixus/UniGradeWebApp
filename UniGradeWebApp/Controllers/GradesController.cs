using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniGradeWebApp;

namespace UniGradeWebApp.Controllers
{
    public class GradesController : Controller
    {
        private readonly DbUniGradeSystemContext _context;

        public GradesController(DbUniGradeSystemContext context)
        {
            _context = context;
        }

        // GET: Grades
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null)
                return RedirectToAction("Students", "Index");
            ViewBag.StnId = id;
            ViewBag.StnFullName = name;
            //var dbUniGradeSystemContext = _context.Grades.Include(g => g.GrdSbjNavigation).Include(g => g.GrdStnNavigation);
            var gradesByStudents = _context.Grades.Where(g => g.GrdStn == id).Include(g => g.GrdStnNavigation);
            return View(await gradesByStudents.ToListAsync());
        }

        // GET: Grades/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Grades == null)
            {
                return NotFound();
            }

            var grade = await _context.Grades
                .Include(g => g.GrdSbjNavigation)
                .Include(g => g.GrdStnNavigation)
                .FirstOrDefaultAsync(m => m.GrdId == id);
            if (grade == null)
            {
                return NotFound();
            }

            return View(grade);
        }

        // GET: Grades/Create
        public IActionResult Create()
        {
            ViewData["GrdSbj"] = new SelectList(_context.Subjects, "SbjId", "SbjId");
            ViewData["GrdStn"] = new SelectList(_context.Students, "StnId", "StnId");
            return View();
        }

        // POST: Grades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GrdId,GrdSbj,GrdStn,GrdResult")] Grade grade)
        {
            if (ModelState.IsValid)
            {
                _context.Add(grade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GrdSbj"] = new SelectList(_context.Subjects, "SbjId", "SbjId", grade.GrdSbj);
            ViewData["GrdStn"] = new SelectList(_context.Students, "StnId", "StnId", grade.GrdStn);
            return View(grade);
        }

        // GET: Grades/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Grades == null)
            {
                return NotFound();
            }

            var grade = await _context.Grades.FindAsync(id);
            if (grade == null)
            {
                return NotFound();
            }
            ViewData["GrdSbj"] = new SelectList(_context.Subjects, "SbjId", "SbjId", grade.GrdSbj);
            ViewData["GrdStn"] = new SelectList(_context.Students, "StnId", "StnId", grade.GrdStn);
            return View(grade);
        }

        // POST: Grades/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GrdId,GrdSbj,GrdStn,GrdResult")] Grade grade)
        {
            if (id != grade.GrdId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(grade);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GradeExists(grade.GrdId))
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
            ViewData["GrdSbj"] = new SelectList(_context.Subjects, "SbjId", "SbjId", grade.GrdSbj);
            ViewData["GrdStn"] = new SelectList(_context.Students, "StnId", "StnId", grade.GrdStn);
            return View(grade);
        }

        // GET: Grades/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Grades == null)
            {
                return NotFound();
            }

            var grade = await _context.Grades
                .Include(g => g.GrdSbjNavigation)
                .Include(g => g.GrdStnNavigation)
                .FirstOrDefaultAsync(m => m.GrdId == id);
            if (grade == null)
            {
                return NotFound();
            }

            return View(grade);
        }

        // POST: Grades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Grades == null)
            {
                return Problem("Entity set 'DbUniGradeSystemContext.Grades'  is null.");
            }
            var grade = await _context.Grades.FindAsync(id);
            if (grade != null)
            {
                _context.Grades.Remove(grade);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GradeExists(int id)
        {
          return (_context.Grades?.Any(e => e.GrdId == id)).GetValueOrDefault();
        }
    }
}
