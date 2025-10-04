using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillShareHub.Data;
using SkillShareHub.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SkillShareHub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // List all courses with instructor info
        public async Task<IActionResult> Courses()
        {
            var courses = await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Instructor)
                .ToListAsync();

            return View(courses);
        }

        // Approve a course
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            course.Status = CourseStatus.Approved;
            await _context.SaveChangesAsync();

            return RedirectToAction("Courses");
        }

        // Reject a course
        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            course.Status = CourseStatus.Rejected;
            await _context.SaveChangesAsync();

            return RedirectToAction("Courses");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Dashboard()
        {
            // Total Students & Instructors
            var totalStudents = await _userManager.GetUsersInRoleAsync("Student");
            var totalInstructors = await _userManager.GetUsersInRoleAsync("Instructor");

            // Courses stats
            var courses = await _context.Courses.ToListAsync();
            var totalCourses = courses.Count;
            var pendingCourses = courses.Count(c => c.Status == CourseStatus.Pending);
            var approvedCourses = courses.Count(c => c.Status == CourseStatus.Approved);
            var rejectedCourses = courses.Count(c => c.Status == CourseStatus.Rejected);

            // Enrollments
            var totalEnrollments = await _context.Enrollments.CountAsync();

            // Instructor stats (fixed: use UserManager instead of UserRoles nav)
            var instructors = await _userManager.GetUsersInRoleAsync("Instructor");

            var instructorStats = instructors.Select(i => new InstructorStat
            {
                InstructorName = i.UserName,
                CourseCount = _context.Courses.Count(c => c.InstructorId == i.Id),
                StudentCount = _context.Enrollments
                    .Where(e => e.Course.InstructorId == i.Id)
                    .Select(e => e.StudentId)
                    .Distinct()
                    .Count()
            }).ToList();

            // Top 5 courses by enrollments
            var topCourses = await _context.Courses
                .Where(c => c.Status == CourseStatus.Approved)
                .OrderByDescending(c => c.Enrollments.Count)
                .Take(5)
                .ToListAsync();

            // Build ViewModel
            var model = new AdminDashboardViewModel
            {
                TotalStudents = totalStudents.Count,
                TotalInstructors = totalInstructors.Count,
                TotalCourses = totalCourses,
                PendingCourses = pendingCourses,
                ApprovedCourses = approvedCourses,
                RejectedCourses = rejectedCourses,
                TotalEnrollments = totalEnrollments,
                InstructorStats = instructorStats,
                TopCourses = topCourses
            };

            return View(model);
        }


    }
}
