
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SudokuExamProject.DataAccessLayer;
using SudokuExamProject.Models;
using System.Threading.Tasks;

namespace SudokuExamProject.Controllers
{
    public class SignInController : Controller
    {
        
        public IActionResult Index()
        {

            return View();
        }

    }
}
