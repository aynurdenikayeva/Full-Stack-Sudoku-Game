
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SudokuExamProject.DataAccessLayer;
using SudokuExamProject.Models;
using SudokuExamProject.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace SudokuExamProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public DashboardController(
            UserManager<AppUser> userManager,
            AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();

            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            var players = await _userManager.GetUsersInRoleAsync("Player");

            int totalUsers = users.Count;

            int activeUsers = users.Count(u =>
                u.AccountStatus != "Blocked" && u.IsOnline);

            int inactiveUsers = users.Count(u =>
                u.AccountStatus != "Blocked" && !u.IsOnline);

            int blockedUsers = users.Count(u =>
                u.AccountStatus == "Blocked");

            var model = new DashboardViewModel
            {
                TotalUsers = totalUsers,
                AdminCount = admins.Count,
                PlayerCount = players.Count,

                ActiveUsers = activeUsers,
                InactiveUsers = inactiveUsers,
                BlockedUsers = blockedUsers,

                TotalMessages = await _context.ContactMessages.CountAsync(),

                Users = users
                    .OrderBy(u => u.Name)
                    .Take(8)
                    .ToList()
            };

            return View(model);
        }
    }
}
