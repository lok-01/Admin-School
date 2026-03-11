using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Data;
using SchoolManagement.Models;

namespace SchoolManagement.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _db;
        public DashboardController(AppDbContext db) => _db = db;

        public IActionResult Index()
        {
            var vm = new DashboardViewModel
            {
                TotalStudents = _db.Students.Count(),
                TotalCourses = _db.Courses.Count(),
                TotalTeachers = _db.Teachers.Count(),
                TotalSubjects = _db.Subjects.Count(),
                TotalClasses = _db.ClassSections.Count(),
                RecentStudents = _db.Students.OrderByDescending(s => s.StudentId).Take(5).ToList(),
                RecentTeachers = _db.Teachers.OrderByDescending(t => t.TeacherId).Take(5).ToList()
            };
            return View(vm);
        }
    }
}
