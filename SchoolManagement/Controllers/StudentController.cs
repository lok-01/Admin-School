using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Data;
using SchoolManagement.Models;

namespace SchoolManagement.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly AppDbContext _db;
        public StudentController(AppDbContext db) => _db = db;

        public IActionResult Index(string? search)
        {
            ViewBag.Search = search;
            var q = _db.Students.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.ToLower();
                q = q.Where(x => x.Name.ToLower().Contains(s) || x.Email.ToLower().Contains(s) || x.Mobile.Contains(s));
            }
            return View(q.OrderByDescending(x => x.StudentId).ToList());
        }

        public IActionResult Create() => View(new Student());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student model)
        {
            if (!ModelState.IsValid) return View(model);
            _db.Students.Add(model);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Student '{model.Name}' added successfully!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var s = _db.Students.Find(id);
            if (s == null) return NotFound();
            return View(s);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student model)
        {
            if (id != model.StudentId) return BadRequest();
            if (!ModelState.IsValid) return View(model);
            _db.Students.Update(model);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Student '{model.Name}' updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var s = _db.Students.Find(id);
            if (s == null) return NotFound();
            return View(s);
        }

        public IActionResult Delete(int id)
        {
            var s = _db.Students.Find(id);
            if (s == null) return NotFound();
            return View(s);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var s = _db.Students.Find(id);
            if (s != null)
            {
                _db.Students.Remove(s);
                await _db.SaveChangesAsync();
                TempData["Success"] = $"Student '{s.Name}' deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
