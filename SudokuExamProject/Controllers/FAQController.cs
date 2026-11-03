using Microsoft.AspNetCore.Mvc;

namespace SudokuExamProject.Controllers
{
    public class FAQController:Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}
