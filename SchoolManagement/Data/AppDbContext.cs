using Microsoft.EntityFrameworkCore;
using SchoolManagement.Models;
using System.Security.Cryptography;
using System.Text;

namespace SchoolManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<ClassSection> ClassSections { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Subject>()
                .HasOne(s => s.Course)
                .WithMany(c => c.Subjects)
                .HasForeignKey(s => s.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public static class DbSeeder
    {
        public static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        public static void Seed(AppDbContext db)
        {
            if (!db.AdminUsers.Any())
            {
                db.AdminUsers.Add(new AdminUser
                {
                    Username = "admin",
                    PasswordHash = HashPassword("Admin@123"),
                    Email = "admin@school.com"
                });
                db.SaveChanges();
            }

            if (!db.Courses.Any())
            {
                db.Courses.AddRange(
                    new Course { CourseName = "Computer Science", Duration = "3 Years", Fees = 45000 },
                    new Course { CourseName = "Business Administration", Duration = "2 Years", Fees = 35000 },
                    new Course { CourseName = "Electronics Engineering", Duration = "4 Years", Fees = 55000 }
                );
                db.SaveChanges();
            }

            if (!db.ClassSections.Any())
            {
                db.ClassSections.AddRange(
                    new ClassSection { ClassName = "Class 10", Section = "A" },
                    new ClassSection { ClassName = "Class 10", Section = "B" },
                    new ClassSection { ClassName = "Class 11", Section = "A" },
                    new ClassSection { ClassName = "Class 12", Section = "A" }
                );
                db.SaveChanges();
            }

            if (!db.Subjects.Any())
            {
                var cs = db.Courses.First(c => c.CourseName == "Computer Science").CourseId;
                var ba = db.Courses.First(c => c.CourseName == "Business Administration").CourseId;
                var ee = db.Courses.First(c => c.CourseName == "Electronics Engineering").CourseId;
                db.Subjects.AddRange(
                    new Subject { SubjectName = "Programming Fundamentals", CourseId = cs },
                    new Subject { SubjectName = "Data Structures", CourseId = cs },
                    new Subject { SubjectName = "Database Management", CourseId = cs },
                    new Subject { SubjectName = "Marketing Management", CourseId = ba },
                    new Subject { SubjectName = "Financial Accounting", CourseId = ba },
                    new Subject { SubjectName = "Circuit Theory", CourseId = ee },
                    new Subject { SubjectName = "Digital Electronics", CourseId = ee }
                );
                db.SaveChanges();
            }

            if (!db.Teachers.Any())
            {
                db.Teachers.AddRange(
                    new Teacher { Name = "Dr. Anand Rao", Mobile = "9876543220", Email = "anand@school.com", Subject = "Mathematics" },
                    new Teacher { Name = "Mrs. Lakshmi Devi", Mobile = "9876543221", Email = "lakshmi@school.com", Subject = "Science" },
                    new Teacher { Name = "Mr. Suresh Babu", Mobile = "9876543222", Email = "suresh@school.com", Subject = "Computer Science" }
                );
                db.SaveChanges();
            }

            if (!db.Students.Any())
            {
                db.Students.AddRange(
                    new Student { Name = "Ravi Kumar", DOB = new DateTime(2005, 3, 15), Gender = "Male", Mobile = "9876543210", Email = "ravi@example.com", Address = "123 MG Road, Hyderabad" },
                    new Student { Name = "Priya Sharma", DOB = new DateTime(2006, 7, 22), Gender = "Female", Mobile = "9876543211", Email = "priya@example.com", Address = "456 Banjara Hills, Hyderabad" }
                );
                db.SaveChanges();
            }
        }
    }
}
