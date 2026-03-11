using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Data;
using SchoolManagement.Models;

namespace SchoolManagement.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly AppDbContext _db;
        public CourseController(AppDbContext db) => _db = db;

        public IActionResult Index(string? search)
        {
            ViewBag.Search = search;
            var q = _db.Courses.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
                q = q.Where(c => c.CourseName.ToLower().Contains(search.ToLower()) || c.Duration.ToLower().Contains(search.ToLower()));
            return View(q.OrderBy(c => c.CourseName).ToList());
        }

        public IActionResult Create() => View(new Course());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course model)
        {
            if (!ModelState.IsValid) return View(model);
            _db.Courses.Add(model);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Course '{model.CourseName}' added successfully!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var c = _db.Courses.Find(id);
            if (c == null) return NotFound();
            return View(c);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Course model)
        {
            if (id != model.CourseId) return BadRequest();
            if (!ModelState.IsValid) return View(model);
            _db.Courses.Update(model);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Course '{model.CourseName}' updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var c = _db.Courses.Find(id);
            if (c == null) return NotFound();
            return View(c);
        }

        public IActionResult Delete(int id)
        {
            var c = _db.Courses.Find(id);
            if (c == null) return NotFound();
            return View(c);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var c = _db.Courses.Find(id);
            if (c != null)
            {
                _db.Courses.Remove(c);
                await _db.SaveChangesAsync();
                TempData["Success"] = $"Course '{c.CourseName}' deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
