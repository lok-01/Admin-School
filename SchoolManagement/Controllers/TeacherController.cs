using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Data;
using SchoolManagement.Models;

namespace SchoolManagement.Controllers
{
    [Authorize]
    public class TeacherController : Controller
    {
        private readonly AppDbContext _db;
        public TeacherController(AppDbContext db) => _db = db;

        public IActionResult Index(string? search)
        {
            ViewBag.Search = search;
            var q = _db.Teachers.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.ToLower();
                q = q.Where(t => t.Name.ToLower().Contains(s) || t.Email.ToLower().Contains(s) || t.Subject.ToLower().Contains(s));
            }
            return View(q.OrderBy(t => t.Name).ToList());
        }

        public IActionResult Create() => View(new Teacher());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Teacher model)
        {
            if (!ModelState.IsValid) return View(model);
            _db.Teachers.Add(model);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Teacher '{model.Name}' added successfully!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var t = _db.Teachers.Find(id);
            if (t == null) return NotFound();
            return View(t);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Teacher model)
        {
            if (id != model.TeacherId) return BadRequest();
            if (!ModelState.IsValid) return View(model);
            _db.Teachers.Update(model);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Teacher '{model.Name}' updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var t = _db.Teachers.Find(id);
            if (t == null) return NotFound();
            return View(t);
        }

        public IActionResult Delete(int id)
        {
            var t = _db.Teachers.Find(id);
            if (t == null) return NotFound();
            return View(t);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var t = _db.Teachers.Find(id);
            if (t != null)
            {
                _db.Teachers.Remove(t);
                await _db.SaveChangesAsync();
                TempData["Success"] = $"Teacher '{t.Name}' deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
