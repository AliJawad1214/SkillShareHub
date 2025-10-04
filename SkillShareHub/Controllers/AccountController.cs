using Microsoft.AspNetCore.Mvc;

namespace SkillShareHub.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult ChooseRole()
        {
            return View();
        }
    }
}
