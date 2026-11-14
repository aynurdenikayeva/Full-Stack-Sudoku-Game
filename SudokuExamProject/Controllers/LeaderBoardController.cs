
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SudokuExamProject.DataAccessLayer;
using SudokuExamProject.Models;
using SudokuExamProject.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SudokuExamProject.Controllers
{
    public class LeaderboardController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public LeaderboardController(
            AppDbContext context,
            UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string difficulty = "Asan")
        {
            var allowedDifficulties = new[]
            {
                "Hamısı",
                "Asan",
                "Orta",
                "Çətin",
                "Ekspert",
                "Usta",
                "Ekstremal"
            };

            if (!allowedDifficulties.Contains(
                difficulty,
                StringComparer.OrdinalIgnoreCase))
            {
                difficulty = "Asan";
            }
            else
            {
                difficulty = allowedDifficulties.First(x =>
                    string.Equals(
                        x,
                        difficulty,
                        StringComparison.OrdinalIgnoreCase));
            }

            var query = _context.SudokuResults
                .AsNoTracking()
                .Include(x => x.User)
                .AsQueryable();

            if (difficulty != "Hamısı")
            {
                query = query.Where(x =>
                    x.Difficulty == difficulty);
            }

            var results = await query.ToListAsync();

            var currentUserId = _userManager.GetUserId(User);

            var entries = results
                .GroupBy(x => x.UserId)
                .Select(group =>
                {
                    var bestResult = group
                        .OrderBy(x => x.ElapsedSeconds)
                        .ThenBy(x => x.Mistakes)
                        .ThenBy(x => x.CompletedAt)
                        .First();

                    var user = group.First().User;

                    return new LeaderboardEntryViewModel
                    {
                        UserId = group.Key,

                        PlayerName =
                            !string.IsNullOrWhiteSpace(user?.Name)
                                ? user.Name
                                : !string.IsNullOrWhiteSpace(user?.UserName)
                                    ? user.UserName
                                    : user?.Email ?? "Oyunçu",

                        GamesPlayed = group.Count(),

                        BestTimeSeconds =
                            bestResult.ElapsedSeconds,

                        BestRunMistakes =
                            bestResult.Mistakes,

                        AverageTimeSeconds =
                            group.Average(x => x.ElapsedSeconds),

                        TotalMistakes =
                            group.Sum(x => x.Mistakes),

                        LastPlayedAt =
                            group.Max(x => x.CompletedAt),

                        IsCurrentUser =
                            currentUserId != null &&
                            group.Key == currentUserId
                    };
                })
                .OrderBy(x => x.BestTimeSeconds)
                .ThenBy(x => x.BestRunMistakes)
                .ThenByDescending(x => x.GamesPlayed)
                .ThenBy(x => x.PlayerName)
                .ToList();

            for (int i = 0; i < entries.Count; i++)
            {
                entries[i].Rank = i + 1;
            }

            var currentUserEntry = entries.FirstOrDefault(
                x => x.IsCurrentUser);

            var model = new LeaderboardViewModel
            {
                SelectedDifficulty = difficulty,
                Entries = entries,
                CurrentUserEntry = currentUserEntry,
                CurrentUserRank = currentUserEntry?.Rank
            };

            return View(model);
        }
    }
}
