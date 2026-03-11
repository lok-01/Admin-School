using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Models
{
    public class AdminUser
    {
        [Key] public int Id { get; set; }
        [Required, MaxLength(100)] public string Username { get; set; } = "";
        [Required] public string PasswordHash { get; set; } = "";
        [Required, EmailAddress, MaxLength(200)] public string Email { get; set; } = "";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public class Student
    {
        [Key] public int StudentId { get; set; }
        [Required, MaxLength(150), Display(Name = "Full Name")] public string Name { get; set; } = "";
        [Required, Display(Name = "Date of Birth")] public DateTime DOB { get; set; }
        [Required, MaxLength(10)] public string Gender { get; set; } = "";
        [Required, MaxLength(15), Display(Name = "Mobile")] public string Mobile { get; set; } = "";
        [Required, EmailAddress, MaxLength(200)] public string Email { get; set; } = "";
        [Required, MaxLength(500)] public string Address { get; set; } = "";
    }

    public class Course
    {
        [Key] public int CourseId { get; set; }
        [Required, MaxLength(150), Display(Name = "Course Name")] public string CourseName { get; set; } = "";
        [Required, MaxLength(50)] public string Duration { get; set; } = "";
        [Required, Column(TypeName = "decimal(18,2)"), Range(0, 9999999)] public decimal Fees { get; set; }
        public ICollection<Subject>? Subjects { get; set; }
    }

    public class ClassSection
    {
        [Key] public int ClassId { get; set; }
        [Required, MaxLength(100), Display(Name = "Class Name")] public string ClassName { get; set; } = "";
        [Required, MaxLength(10)] public string Section { get; set; } = "";
    }

    public class Subject
    {
        [Key] public int SubjectId { get; set; }
        [Required, MaxLength(150), Display(Name = "Subject Name")] public string SubjectName { get; set; } = "";
        [Required, Display(Name = "Course")] public int CourseId { get; set; }
        public Course? Course { get; set; }
    }

    public class Teacher
    {
        [Key] public int TeacherId { get; set; }
        [Required, MaxLength(150)] public string Name { get; set; } = "";
        [Required, MaxLength(15), Display(Name = "Mobile")] public string Mobile { get; set; } = "";
        [Required, EmailAddress, MaxLength(200)] public string Email { get; set; } = "";
        [Required, MaxLength(200), Display(Name = "Subject Specialization")] public string Subject { get; set; } = "";
    }

    // View Models
    public class LoginViewModel
    {
        [Required] public string Username { get; set; } = "";
        [Required, DataType(DataType.Password)] public string Password { get; set; } = "";
    }

    public class ChangePasswordViewModel
    {
        [Required, DataType(DataType.Password), Display(Name = "Current Password")] public string CurrentPassword { get; set; } = "";
        [Required, DataType(DataType.Password), Display(Name = "New Password"), MinLength(6)] public string NewPassword { get; set; } = "";
        [Required, DataType(DataType.Password), Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")] public string ConfirmPassword { get; set; } = "";
    }

    public class DashboardViewModel
    {
        public int TotalStudents { get; set; }
        public int TotalCourses { get; set; }
        public int TotalTeachers { get; set; }
        public int TotalSubjects { get; set; }
        public int TotalClasses { get; set; }
        public List<Student> RecentStudents { get; set; } = new();
        public List<Teacher> RecentTeachers { get; set; } = new();
    }
}
