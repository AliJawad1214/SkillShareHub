using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SkillShareHub.Controllers
{
    [Authorize(Roles = "Instructor")]   // Only Instructors can access
    public class InstructorController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
