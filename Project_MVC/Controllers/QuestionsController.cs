using Microsoft.AspNetCore.Mvc;

namespace Project_MVC.Controllers
{
    public class QuestionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult QuestionDetail()
        {
            return View();
        }
    }
}
