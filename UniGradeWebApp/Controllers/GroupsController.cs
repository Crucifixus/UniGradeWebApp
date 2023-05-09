using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using UniGradeWebApp;

namespace UniGradeWebApp.Controllers
{
    public class GroupsController : Controller
    {
        private readonly DbUniGradeSystemContext _context;

        public GroupsController(DbUniGradeSystemContext context)
        {
            _context = context;
        }

        // GET: Groups
        public async Task<IActionResult> Index(int? id, string? name, int? facid, string? facname)
        {
            if(id == null)
                return RedirectToAction("Cathedras", "Index");
            ViewBag.CathId = id;
            ViewBag.CathName = name;
            ViewBag.FacId = facid;
            ViewBag.FacName = facname;
            var groupsByCathedra = _context.Groups.Where(g => g.GrpCath == id).Include(g => g.GrpCathNavigation);

            return View(await groupsByCathedra.ToListAsync());
        }

        // GET: Groups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups
                .Include(g => g.GrpCathNavigation)
                .FirstOrDefaultAsync(m => m.GrpId == id);
            if (@group == null)
            {
                return NotFound();
            }

            //return View(@group);
            ViewBag.CathId = group.GrpCath;
            ViewBag.CathName = _context.Cathedras.Where(f => f.CathId == group.GrpCath).FirstOrDefault().CathName;
            return RedirectToAction("Index", "Students", new {id = group.GrpId, name = group.GrpName, enrollmentyear = group.GrpEnrollmentYear});
        }

        // GET: Groups/Create
        public IActionResult Create(int CathId)
        {
            //ViewData["GrpCath"] = new SelectList(_context.Cathedras, "CathId", "CathId");
            ViewBag.CathId = CathId;
            ViewBag.Cathedra = _context.Cathedras.Where(f => f.CathId == CathId).FirstOrDefault();
            ViewBag.CathName = ViewBag.Cathedra.CathName;
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int CathId, [Bind("GrpId,GrpName,GrpEnrollmentYear,GrpCath")] Group @group)
        {
            group.GrpCath = CathId;
            var cath = ViewBag.Cathedra = _context.Cathedras.Where(f => f.CathId == CathId).FirstOrDefault();
            if (ModelState.IsValid)
            {
                _context.Add(@group);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Groups", new { id = CathId, name = cath.CathName });
            }
            //ViewData["GrpCath"] = new SelectList(_context.Cathedras, "CathId", "CathId", @group.GrpCath);
            return View(group);
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups.FindAsync(id);
            if (@group == null)
            {
                return NotFound();
            }
            ViewBag.CathId = group.GrpCath;
            ViewBag.CathName = _context.Cathedras.Where(f => f.CathId == group.GrpCath).FirstOrDefault().CathName;
            ViewData["GrpCath"] = new SelectList(_context.Cathedras, "CathId", "CathId", @group.GrpCath);
            return View(@group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GrpId,GrpName,GrpEnrollmentYear,GrpCath")] Group @group)
        {
            if (id != @group.GrpId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@group);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(@group.GrpId))
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
            ViewBag.CathId = group.GrpCath;
            ViewBag.CathName = _context.Cathedras.Where(f => f.CathId == group.GrpCath).FirstOrDefault().CathName;
            ViewData["GrpCath"] = new SelectList(_context.Cathedras, "CathId", "CathId", @group.GrpCath);
            return View(@group);
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Groups == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups
                .Include(g => g.GrpCathNavigation)
                .FirstOrDefaultAsync(m => m.GrpId == id);
            if (@group == null)
            {
                return NotFound();
            }
            ViewBag.CathId = group.GrpCath;
            ViewBag.CathName = _context.Cathedras.Where(f => f.CathId == group.GrpCath).FirstOrDefault().CathName;
            return View(@group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Groups == null)
            {
                return Problem("Entity set 'DbUniGradeSystemContext.Groups'  is null.");
            }
            var @group = await _context.Groups
                .Include(g => g.Students)
                .FirstOrDefaultAsync(m => m.GrpId == id);
            if (group != null
                && (group.Students == null || group.Students.Count == 0))
            {
                _context.Groups.Remove(group);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ErrMessage = "Елемент не повинен мати дочірніх.";
            ViewBag.CathId = group.GrpCath;
            ViewBag.CathName = _context.Cathedras.Where(f => f.CathId == group.GrpCath).FirstOrDefault().CathName;
            return View(group);
        }

        private bool GroupExists(int id)
        {
          return (_context.Groups?.Any(e => e.GrpId == id)).GetValueOrDefault();
        }
    }
}
