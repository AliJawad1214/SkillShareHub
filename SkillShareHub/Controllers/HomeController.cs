using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SkillShareHub.Data;
using SkillShareHub.Models;
using System.Diagnostics;

namespace SkillShareHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? search, int? categoryId)
        {
            var baseQuery = _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Instructor)
                .Where(c => c.Status == CourseStatus.Approved)
                .AsQueryable();

            // Featured courses (latest 3)
            var featuredCourses = await baseQuery
                .OrderByDescending(c => c.CreatedAt)
                .Take(3)
                .ToListAsync();

            // Exclude featured
            var featuredIds = featuredCourses.Select(c => c.Id).ToList();
            var coursesQuery = baseQuery.Where(c => !featuredIds.Contains(c.Id));

            // 🔍 Apply search
            if (!string.IsNullOrEmpty(search))
            {
                coursesQuery = coursesQuery.Where(c =>
                    c.Title.Contains(search) ||
                    c.Description.Contains(search) ||
                    c.Category.Name.Contains(search));
            }

            // 📂 Apply category filter
            if (categoryId.HasValue)
            {
                coursesQuery = coursesQuery.Where(c => c.CategoryId == categoryId.Value);
            }

            var courses = await coursesQuery.ToListAsync();

            // Pass categories for dropdown
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", categoryId);

            ViewBag.FeaturedCourses = featuredCourses;

            return View(courses);
        }




        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
