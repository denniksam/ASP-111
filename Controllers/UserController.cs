using Microsoft.AspNetCore.Mvc;

namespace ASP_111.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public ViewResult SignUp()
        {
            return View();
        }
    }
}
