using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Data;
using SchoolManagement.Models;

namespace SchoolManagement.Controllers
{
    [Authorize]
    public class ClassController : Controller
    {
        private readonly AppDbContext _db;
        public ClassController(AppDbContext db) => _db = db;

        public IActionResult Index(string? search)
        {
            ViewBag.Search = search;
            var q = _db.ClassSections.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
                q = q.Where(c => c.ClassName.ToLower().Contains(search.ToLower()) || c.Section.ToLower().Contains(search.ToLower()));
            return View(q.OrderBy(c => c.ClassName).ThenBy(c => c.Section).ToList());
        }

        public IActionResult Create() => View(new ClassSection());

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClassSection model)
        {
            if (!ModelState.IsValid) return View(model);
            _db.ClassSections.Add(model);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Class '{model.ClassName} - {model.Section}' added successfully!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var c = _db.ClassSections.Find(id);
            if (c == null) return NotFound();
            return View(c);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClassSection model)
        {
            if (id != model.ClassId) return BadRequest();
            if (!ModelState.IsValid) return View(model);
            _db.ClassSections.Update(model);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Class '{model.ClassName} - {model.Section}' updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            var c = _db.ClassSections.Find(id);
            if (c == null) return NotFound();
            return View(c);
        }

        public IActionResult Delete(int id)
        {
            var c = _db.ClassSections.Find(id);
            if (c == null) return NotFound();
            return View(c);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var c = _db.ClassSections.Find(id);
            if (c != null)
            {
                _db.ClassSections.Remove(c);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Class deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
