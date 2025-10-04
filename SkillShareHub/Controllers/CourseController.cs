using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SkillShareHub.Data;
using SkillShareHub.Models;
using System.Threading.Tasks;

namespace SkillShareHub.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CourseController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Courses/Create
        // GET: Courses/Create
        // GET: Courses/Create
        [Authorize(Roles = "Instructor")]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }



        // POST: Courses/Create
        [Authorize(Roles = "Instructor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            System.Diagnostics.Debug.WriteLine($"Posted CategoryId: {course.CategoryId}");
            // ✅ manually remove Instructor validation errors
            ModelState.Remove("Instructor");
            ModelState.Remove("InstructorId");
            ModelState.Remove("Category");


            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

              //  Console.WriteLine($"InstructorId: {user.Id}");
                System.Diagnostics.Debug.WriteLine($"InstructorId: {user.Id}");

                course.InstructorId = user.Id;
                course.CreatedAt = DateTime.Now;

                _context.Courses.Add(course);
                await _context.SaveChangesAsync();

                return RedirectToAction("MyCourses");
            }

            // If we reach here, ModelState failed
           // Console.WriteLine("ModelState invalid!");
            System.Diagnostics.Debug.WriteLine("ModelState invalid!");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
              //  Console.WriteLine(error.ErrorMessage);
                System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
            }

            // 🔑 Repopulate categories
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", course.CategoryId);

            return View(course);
        }



        // Instructor’s course list
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> MyCourses()
        {
            var user = await _userManager.GetUserAsync(User);
            var courses = _context.Courses
               .Where(c => c.InstructorId == user.Id)
               .Include(c => c.Category) // 👈 include category
               .ToList();
            return View(courses);
        }

        // GET: Courses/Edit/5
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var course = await _context.Courses.FindAsync(id);

            if (course == null || course.InstructorId != user.Id)
            {
                return NotFound(); // or Forbid()
            }

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", course.CategoryId);
            return View(course);
        }

        [Authorize(Roles = "Instructor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Course course)
        {
            var user = await _userManager.GetUserAsync(User);

            if (id != course.Id) return NotFound();

            // prevent editing another instructor’s course
            var existingCourse = await _context.Courses.FindAsync(id);
            if (existingCourse == null || existingCourse.InstructorId != user.Id)
            {
                return Forbid();
            }

            ModelState.Remove("Instructor");
            ModelState.Remove("InstructorId");
            ModelState.Remove("Category");
            if (ModelState.IsValid)
            {
                existingCourse.Title = course.Title;
                existingCourse.Description = course.Description;
                existingCourse.Price = course.Price;
                existingCourse.ImageUrl = course.ImageUrl;
                existingCourse.CategoryId = course.CategoryId;

                await _context.SaveChangesAsync();
                return RedirectToAction("MyCourses");
            }

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", course.CategoryId);
            return View(course);
        }

        // GET: Courses/Delete/5
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var course = await _context.Courses
                .Include(c => c.Category)
                .FirstOrDefaultAsync(c => c.Id == id && c.InstructorId == user.Id);

            if (course == null) return NotFound();

            return View(course);
        }

        [Authorize(Roles = "Instructor")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var course = await _context.Courses.FindAsync(id);

            if (course == null || course.InstructorId != user.Id)
            {
                return Forbid();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction("MyCourses");
        }

        // GET: Courses/Details/5
        [AllowAnonymous] // anyone (even not logged in) can view course details
        public async Task<IActionResult> Details(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(c => c.Id == id && c.Status == CourseStatus.Approved);

            if (course == null)
            {
                return NotFound();
            }

            bool isEnrolled = false;

            if (User.Identity.IsAuthenticated && User.IsInRole("Student"))
            {
                var user = await _userManager.GetUserAsync(User);
                isEnrolled = await _context.Enrollments
                    .AnyAsync(e => e.CourseId == id && e.StudentId == user.Id);
            }

            ViewBag.IsEnrolled = isEnrolled;

            return View(course);
        }

        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enroll(int courseId)
        {
            var user = await _userManager.GetUserAsync(User);

            // Prevent duplicate enrollments
            var alreadyEnrolled = _context.Enrollments
                .Any(e => e.CourseId == courseId && e.StudentId == user.Id);

            if (!alreadyEnrolled)
            {
                var enrollment = new Enrollment
                {
                    CourseId = courseId,
                    StudentId = user.Id,
                    EnrolledAt = DateTime.UtcNow
                };
                _context.Enrollments.Add(enrollment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("MyEnrollments", "Student"); // later we’ll make Student Dashboard
        }


    }
}
