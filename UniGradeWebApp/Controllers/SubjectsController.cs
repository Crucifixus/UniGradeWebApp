﻿using System;
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
    public class SubjectsController : Controller
    {
        private readonly DbUniGradeSystemContext _context;

        public SubjectsController(DbUniGradeSystemContext context)
        {
            _context = context;
        }

        // GET: Subjects
        public async Task<IActionResult> Index(int? id, string? name, int? facid, string? facname)
        {
            if (id == null)
                return RedirectToAction("Cathedras", "Index");
            ViewBag.CathId = id;
            ViewBag.CathName = name;
            ViewBag.FacId = facid;
            ViewBag.FacName = facname;
            //var dbUniGradeSystemContext = _context.Subjects.Include(s => s.SbjCathNavigation);
            var subjectsByCathedra = _context.Subjects.Where(s => s.SbjCath == id).Include(s => s.SbjCathNavigation);
            return View(await subjectsByCathedra.ToListAsync());
        }

        // GET: Subjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Subjects == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .Include(s => s.SbjCathNavigation)
                .FirstOrDefaultAsync(m => m.SbjId == id);
            if (subject == null)
            {
                return NotFound();
            }
            ViewBag.CathId = subject.SbjCath;
            ViewBag.CathName = subject.SbjCathNavigation.CathName;
            return View(subject);
        }

        // GET: Subjects/Create
        public IActionResult Create(int CathId)
        {
            //ViewData["SbjCath"] = new SelectList(_context.Cathedras, "CathId", "CathName");
            ViewBag.CathId = CathId;
            ViewBag.Cathedra = _context.Cathedras.Where(f => f.CathId == CathId).FirstOrDefault();
            ViewBag.CathName = ViewBag.Cathedra.CathName;
            return View();
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int CathId, [Bind("SbjId,SbjName,SbjCath,SbjTeach")] Subject subject)
        {
            subject.SbjCath = CathId;
            var cath = ViewBag.Cathedra = _context.Cathedras.Where(f => f.CathId == CathId).FirstOrDefault();
            if (ModelState.IsValid)
            {
                _context.Add(subject);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Subjects", new { id = CathId, name = cath.CathName });
            }
            //ViewData["SbjCath"] = new SelectList(_context.Cathedras, "CathId", "CathName", subject.SbjCath);
            return View(subject);
        }

        // GET: Subjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Subjects == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            ViewBag.CathId = subject.SbjCath;
            ViewBag.CathName = _context.Cathedras.Where(f => f.CathId == subject.SbjCath).FirstOrDefault().CathName;
            ViewData["SbjCath"] = new SelectList(_context.Cathedras, "CathId", "CathName", subject.SbjCath);
            return View(subject);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SbjId,SbjName,SbjCath,SbjTeach")] Subject subject)
        {
            if (id != subject.SbjId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectExists(subject.SbjId))
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
            ViewBag.CathId = subject.SbjCath;
            ViewBag.CathName = _context.Cathedras.Where(f => f.CathId == subject.SbjCath).FirstOrDefault().CathName;
            ViewData["SbjCath"] = new SelectList(_context.Cathedras, "CathId", "CathName", subject.SbjCath);
            return View(subject);
        }

        // GET: Subjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Subjects == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .Include(s => s.SbjCathNavigation)
                .FirstOrDefaultAsync(m => m.SbjId == id);
            if (subject == null)
            {
                return NotFound();
            }
            ViewBag.CathId = subject.SbjCath;
            ViewBag.CathName = _context.Cathedras.Where(f => f.CathId == subject.SbjCath).FirstOrDefault().CathName;
            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Subjects == null)
            {
                return Problem("Entity set 'DbUniGradeSystemContext.Subjects'  is null.");
            }
            var subject = await _context.Subjects
                .Include(s => s.Grades)
                .FirstOrDefaultAsync(s => s.SbjId == id);
            if (subject != null
                && (subject.Grades == null || subject.Grades.Count == 0))
            {
                _context.Subjects.Remove(subject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ErrMessage = "Елемент не повинен мати дочірніх.";
            ViewBag.CathId = subject.SbjCath;
            ViewBag.CathName = _context.Cathedras.Where(f => f.CathId == subject.SbjCath).FirstOrDefault().CathName;
            return View(subject);
        }

        private bool SubjectExists(int id)
        {
          return (_context.Subjects?.Any(e => e.SbjId == id)).GetValueOrDefault();
        }
    }
}
