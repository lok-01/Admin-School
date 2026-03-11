using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models;

namespace SchoolManagement.Controllers
{
    [Authorize]
    public class SubjectController : Controller
    {
        private readonly AppDbContext _db;
        public SubjectController(AppDbContext db) => _db = db;

        public IActionResult Index(string? search)
        {
            ViewBag.Search = search;
            var q = _db.Subjects.Include(s => s.Course).AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
                q = q.Where(s => s.SubjectName.ToLower().Contains(search.ToLower()) || s.Course!.CourseName.ToLower().Contains(search.ToLower()));
            return View(q.OrderBy(s => s.SubjectName).ToList());
        }

        void LoadCourses(int? selected = null) =>
            ViewBag.Courses = new SelectList(_db.Courses.OrderBy(c => c.CourseName), "CourseId", "CourseName", selected);

        public IActionResult Create() { LoadCourses(); return View(new Subject()); }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Subject model)
        {
            if (!ModelState.IsValid) { LoadCourses(model.CourseId); return View(model); }
            _db.Subjects.Add(model);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Subject '{model.SubjectName}' added successfully!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var s = _db.Subjects.Find(id);
            if (s == null) return NotFound();
            LoadCourses(s.CourseId);
            return View(s);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Subject model)
        {
            if (id != model.SubjectId) return BadRequest();
            if (!ModelState.IsValid) { LoadCourses(model.CourseId); return View(model); }
            _db.Subjects.Update(model);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Subject '{model.SubjectName}' updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var s = _db.Subjects.Include(x => x.Course).FirstOrDefault(x => x.SubjectId == id);
            if (s == null) return NotFound();
            return View(s);
        }

        public IActionResult Delete(int id)
        {
            var s = _db.Subjects.Include(x => x.Course).FirstOrDefault(x => x.SubjectId == id);
            if (s == null) return NotFound();
            return View(s);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var s = _db.Subjects.Find(id);
            if (s != null)
            {
                _db.Subjects.Remove(s);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Subject deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
