using Microsoft.AspNetCore.Mvc;
using SudokuExamProject.Services;

namespace SudokuExamProject.Controllers
{
    public class AboutController:Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}

