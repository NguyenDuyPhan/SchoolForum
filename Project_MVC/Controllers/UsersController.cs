using Microsoft.AspNetCore.Mvc;

namespace Project_MVC.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(string? username, string? password)
        {
            return View();
        }


        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Profile()
        {

            return View();
        }
    }
}
