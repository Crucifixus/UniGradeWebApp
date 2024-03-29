﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using UniGradeWebApp;

namespace UniGradeWebApp.Controllers
{
    public class StudentsController : Controller
    {
        private readonly DbUniGradeSystemContext _context;

        public StudentsController(DbUniGradeSystemContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(int? id, string? name, short? enrollmentyear, int? cathid, string? cathname)
        {
            if (id == null)
                return RedirectToAction("Groups", "Index");
            ViewBag.GrpId = id;
            ViewBag.GrpName = name;
            ViewBag.GrpEnrollmentYear = enrollmentyear;
            ViewBag.CathId = cathid;
            ViewBag.CathName = cathname;
            //var dbUniGradeSystemContext = _context.Students.Include(s => s.StnGrpNavigation);
            var studentsByGroup = _context.Students.Where(s => s.StnGrp == id).Include(s => s.StnGrpNavigation);
            return View(await studentsByGroup.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.StnGrpNavigation)
                .FirstOrDefaultAsync(m => m.StnId == id);
            if (student == null)
            {
                return NotFound();
            }

            ViewBag.GrpId = student.StnGrp;
            ViewBag.GrpName = _context.Groups.Where(f => f.GrpId == student.StnGrp).FirstOrDefault().GrpName;
            ViewBag.GrpEnrollmentYear = _context.Groups.Where(f => f.GrpId == student.StnGrp).FirstOrDefault().GrpEnrollmentYear;
            return RedirectToAction("Index", "Grades", new {id = student.StnId, name = student.StnFullName, grpid = student.StnGrpNavigation.GrpId, grpname = student.StnGrpNavigation.GrpName });
        }

        // GET: Students/Create
        public IActionResult Create(int GrpId)
        {
            //ViewData["StnGrp"] = new SelectList(_context.Groups, "GrpId", "GrpId");
            ViewBag.GrpId = GrpId;
            var grp = _context.Groups.Where(f => f.GrpId == GrpId).FirstOrDefault();
            ViewBag.GrpName = grp.GrpName;
            ViewBag.GrpEnrollmentYear = grp.GrpEnrollmentYear;
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int GrpId, [Bind("StnId,StnFullName,StnGrp")] Student student)
        {
            student.StnGrp = GrpId;
            var grp = _context.Groups.Where(f => f.GrpId == GrpId).FirstOrDefault();
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Students", new { id = GrpId, name = grp.GrpName, enrollmentyear = grp.GrpEnrollmentYear });
            }
            //ViewData["StnGrp"] = new SelectList(_context.Groups, "GrpId", "GrpId", student.StnGrp);
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewBag.GrpId = student.StnGrp;
            ViewBag.GrpName = _context.Groups.Where(f => f.GrpId == student.StnGrp).FirstOrDefault().GrpName;
            ViewBag.GrpEnrollmentYear = _context.Groups.Where(f => f.GrpId == student.StnGrp).FirstOrDefault().GrpEnrollmentYear;
            ViewData["StnGrp"] = new SelectList(_context.Groups, "GrpId", "GrpId", student.StnGrp);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StnId,StnFullName,StnGrp")] Student student)
        {
            if (id != student.StnId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StnId))
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
            ViewBag.GrpId = student.StnGrp;
            ViewBag.GrpName = _context.Groups.Where(f => f.GrpId == student.StnGrp).FirstOrDefault().GrpName;
            ViewBag.GrpEnrollmentYear = _context.Groups.Where(f => f.GrpId == student.StnGrp).FirstOrDefault().GrpEnrollmentYear;
            ViewData["StnGrp"] = new SelectList(_context.Groups, "GrpId", "GrpId", student.StnGrp);
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.StnGrpNavigation)
                .FirstOrDefaultAsync(m => m.StnId == id);
            if (student == null)
            {
                return NotFound();
            }
            ViewBag.GrpId = student.StnGrp;
            ViewBag.GrpName = _context.Groups.Where(f => f.GrpId == student.StnGrp).FirstOrDefault().GrpName;
            ViewBag.GrpEnrollmentYear = _context.Groups.Where(f => f.GrpId == student.StnGrp).FirstOrDefault().GrpEnrollmentYear;
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Students == null)
            {
                return Problem("Entity set 'DbUniGradeSystemContext.Students'  is null.");
            }
            var student = await _context.Students
                .Include(s => s.Grades)
                .FirstOrDefaultAsync(s => s.StnId == id);
            if (student != null
                && (student.Grades == null || student.Grades.Count == 0))
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ErrMessage = "Елемент не повинен мати дочірніх.";
            ViewBag.GrpId = student.StnGrp;
            ViewBag.GrpName = _context.Groups.Where(f => f.GrpId == student.StnGrp).FirstOrDefault().GrpName;
            ViewBag.GrpEnrollmentYear = _context.Groups.Where(f => f.GrpId == student.StnGrp).FirstOrDefault().GrpEnrollmentYear;
            return View(student);
        }

        private bool StudentExists(int id)
        {
          return (_context.Students?.Any(e => e.StnId == id)).GetValueOrDefault();
        }
    }
}
