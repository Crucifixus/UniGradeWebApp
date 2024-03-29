﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using UniGradeWebApp;

namespace UniGradeWebApp.Controllers
{
    public class CathedrasController : Controller
    {
        private readonly DbUniGradeSystemContext _context;

        public CathedrasController(DbUniGradeSystemContext context)
        {
            _context = context;
        }

        // GET: Cathedras
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) 
                return RedirectToAction("Faculties", "Index");
            ViewBag.FacId = id;
            ViewBag.FacName = name;
            var cathedrasByFaculty = _context.Cathedras.Where(c => c.CathFac == id).Include(c => c.CathFacNavigation);

            return View(await cathedrasByFaculty.ToListAsync());
        }

        // GET: Cathedras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cathedras == null)
            {
                return NotFound();
            }

            var cathedra = await _context.Cathedras
                .Include(c => c.CathFacNavigation)
                .FirstOrDefaultAsync(m => m.CathId == id);
            if (cathedra == null)
            {
                return NotFound();
            }
            ViewBag.FacId = cathedra.CathFac;
            ViewBag.FacName = _context.Faculties.Where(f => f.FacId == cathedra.CathFac).FirstOrDefault().FacName;
            return RedirectToAction("Index", "Groups", new { id = cathedra.CathId, name = cathedra.CathName, facid = cathedra.CathFacNavigation.FacId, facname = cathedra.CathFacNavigation.FacName });
        }

        // GET: Cathedras/Details2/5
        public async Task<IActionResult> Details2(int? id)
        {
            if (id == null || _context.Cathedras == null)
            {
                return NotFound();
            }

            var cathedra = await _context.Cathedras
                .Include(c => c.CathFacNavigation)
                .FirstOrDefaultAsync(m => m.CathId == id);
            if (cathedra == null)
            {
                return NotFound();
            }

            ViewBag.FacId = cathedra.CathFac;
            ViewBag.FacName = _context.Faculties.Where(f => f.FacId == cathedra.CathFac).FirstOrDefault().FacName;
            return RedirectToAction("Index", "Subjects", new { id = cathedra.CathId, name = cathedra.CathName, facid = cathedra.CathFacNavigation.FacId, facname = cathedra.CathFacNavigation.FacName });
        }

        // GET: Cathedras/Create
        public IActionResult Create(int FacId)
        {
            ViewBag.FacId = FacId;
            ViewBag.Faculty = _context.Faculties.Where(f => f.FacId == FacId).FirstOrDefault();
            ViewBag.FacName = ViewBag.Faculty.FacName;
            return View();
        }

        // POST: Cathedras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int FacId, [Bind("CathId,CathName")] Cathedra cathedra)
        {
            cathedra.CathFac = FacId;
            var fac = _context.Faculties.Where(f => f.FacId == FacId).FirstOrDefault();
            //ModelState.ClearValidationState(nameof(Cathedra));
            //if (!TryValidateModel(cathedra, nameof(Cathedra))) 
            //{ }
            //IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                _context.Add(cathedra);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Cathedras", new { id = FacId, name = fac.FacName, foundingyear = fac.FacFoundingYear } );
            }
            return View(cathedra);           
        }

        // GET: Cathedras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cathedras == null)
            {
                return NotFound();
            }

            var cathedra = await _context.Cathedras.FindAsync(id);
            if (cathedra == null)
            {
                return NotFound();
            }
            ViewBag.FacId = cathedra.CathFac;
            ViewBag.FacName = _context.Faculties.Where(f => f.FacId == cathedra.CathFac).FirstOrDefault().FacName;
            ViewData["CathFac"] = new SelectList(_context.Faculties, "FacId", "FacId", cathedra.CathFac);
            return View(cathedra);
        }

        // POST: Cathedras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CathId,CathName,CathFac")] Cathedra cathedra)
        {
            if (id != cathedra.CathId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cathedra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CathedraExists(cathedra.CathId))
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
            ViewBag.FacId = cathedra.CathFac;
            ViewBag.FacName = _context.Faculties.Where(f => f.FacId == cathedra.CathFac).FirstOrDefault().FacName;
            ViewData["CathFac"] = new SelectList(_context.Faculties, "FacId", "FacId", cathedra.CathFac);
            return View(cathedra);
        }

        // GET: Cathedras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cathedras == null)
            {
                return NotFound();
            }

            var cathedra = await _context.Cathedras
                .Include(c => c.CathFacNavigation)
                .FirstOrDefaultAsync(m => m.CathId == id);
            if (cathedra == null)
            {
                return NotFound();
            }
            ViewBag.FacId = cathedra.CathFac;
            ViewBag.FacName = _context.Faculties.Where(f => f.FacId == cathedra.CathFac).FirstOrDefault().FacName;
            return View(cathedra);
        }

        // POST: Cathedras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cathedras == null)
            {
                return Problem("Entity set 'DbUniGradeSystemContext.Cathedras'  is null.");
            }
            var cathedra = await _context.Cathedras
                .Include(c => c.Groups)
                .Include(c => c.Subjects)
                .FirstOrDefaultAsync(m => m.CathId == id);
            if (cathedra != null 
                && (cathedra.Groups == null || cathedra.Groups.Count == 0)
                && (cathedra.Subjects == null || cathedra.Subjects.Count == 0))
            {
                _context.Cathedras.Remove(cathedra);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ErrMessage = "Елемент не повинен мати дочірніх.";
            ViewBag.FacId = cathedra.CathFac;
            ViewBag.FacName = _context.Faculties.Where(f => f.FacId == cathedra.CathFac).FirstOrDefault().FacName;
            return View(cathedra);
        }

        private bool CathedraExists(int id)
        {
          return (_context.Cathedras?.Any(e => e.CathId == id)).GetValueOrDefault();
        }
    }
}
