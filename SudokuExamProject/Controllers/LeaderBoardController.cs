using Microsoft.AspNetCore.Mvc;

namespace SudokuExamProject.Controllers
{
    public class LeaderBoardController:Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}
