using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SudokuExamProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class ChartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
