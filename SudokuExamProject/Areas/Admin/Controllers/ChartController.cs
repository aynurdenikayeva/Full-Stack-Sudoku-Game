using Microsoft.AspNetCore.Mvc;

namespace SudokuExamProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ChartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
