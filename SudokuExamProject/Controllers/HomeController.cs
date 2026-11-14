
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SudokuExamProject.DataAccessLayer;
using SudokuExamProject.Models;
using SudokuExamProject.Services;

namespace SudokuExamProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SudokuGameManager _gameManager;
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(
            ILogger<HomeController> logger,
            SudokuGameManager gameManager,
            AppDbContext context,
            UserManager<AppUser> userManager)
        {
            _logger = logger;
            _gameManager = gameManager;
            _context = context;
            _userManager = userManager;
        }

        private SudokuService CurrentSudoku
        {
            get
            {
                HttpContext.Session.SetString(
                    "SudokuSessionInitialized", "true");

                return _gameManager.GetGame(
                    HttpContext.Session.Id);
            }
        }

        // GET: /
        [HttpGet]
        public IActionResult Index()
        {
            CurrentSudoku.Game.Completed =
                CurrentSudoku.IsCompleted();

            return View(CurrentSudoku.Game);
        }

        // POST: /Home/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(
            int row,
            int col,
            int? number)
        {
            if (!IsValidCell(row, col))
            {
                return BadRequest(new
                {
                    message = "Xana düzgün seçilməyib."
                });
            }

            string message =
                CurrentSudoku.AddNumber(row, col, number);

            CurrentSudoku.Game.Completed =
                CurrentSudoku.IsCompleted();

            await SaveCompletedResultAsync();

            return Json(GetGameResult(message, row, col));
        }

        // POST: /Home/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult New(string difficulty = "Orta")
        {
            CurrentSudoku.NewGame(difficulty);

            return Redirect(Url.Action(nameof(Index)) + "#oyun");
        }

        // POST: /Home/Hint
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Hint(int row, int col)
        {
            if (!IsValidCell(row, col))
            {
                return BadRequest(new
                {
                    message = "Xana düzgün seçilməyib."
                });
            }

            string message =
                CurrentSudoku.UseHint(row, col);

            CurrentSudoku.Game.Completed =
                CurrentSudoku.IsCompleted();

            await SaveCompletedResultAsync();

            return Json(GetGameResult(message, row, col));
        }

        // POST: /Home/Clear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Clear(int row, int col)
        {
            if (!IsValidCell(row, col))
            {
                return BadRequest(new
                {
                    message = "Xana düzgün seçilməyib."
                });
            }

            string message =
                CurrentSudoku.ClearCell(row, col);

            return Json(GetGameResult(message, row, col));
        }

        // POST: /Home/Undo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Undo()
        {
            string message = CurrentSudoku.Undo();

            var game = CurrentSudoku.Game;

            game.Completed =
                CurrentSudoku.IsCompleted();

            return Json(new
            {
                message,
                completed = game.Completed,
                mistakes = game.Mistakes,
                hintsUsed = game.HintsUsed,
                difficulty = game.Difficulty,
                board = GetBoard()
            });
        }

        // Nəticəni PostgreSQL bazasına yazır

        private async Task SaveCompletedResultAsync()
        {
            if (User.Identity == null ||
                !User.Identity.IsAuthenticated)
                return;

            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
                return;

            var sessionId = HttpContext.Session.Id;

            await _gameManager.SaveResultOnceAsync(
                sessionId,
                async () =>
                {
                    var game = CurrentSudoku.Game;

                    var result = new SudokuResult
                    {
                        UserId = userId,
                        ElapsedSeconds =
                            CurrentSudoku.GetElapsedSeconds(),
                        Mistakes = game.Mistakes,
                        HintsUsed = game.HintsUsed,
                        Difficulty = game.Difficulty,
                        CompletedAt = DateTime.UtcNow
                    };

                    _context.SudokuResults.Add(result);

                    try
                    {
                        await _context.SaveChangesAsync();

                        _logger.LogInformation(
                            "Sudoku nəticəsi yadda saxlanıldı. UserId: {UserId}",
                            userId);
                    }
                    catch
                    {
                        _context.Entry(result).State =
                            EntityState.Detached;

                        throw;
                    }
                });
        }


        // AJAX cavabı
        private object GetGameResult(
            string message,
            int row,
            int col)
        {
            var game = CurrentSudoku.Game;

            return new
            {
                message,
                row,
                col,
                value = game.Board[row, col],
                completed = game.Completed,
                mistakes = game.Mistakes,
                hintsUsed = game.HintsUsed,
                difficulty = game.Difficulty,
                board = GetBoard()
            };
        }

        // Sudoku lövhəsini JSON formatına çevirir
        private int[][] GetBoard()
        {
            var board = new int[9][];

            for (int row = 0; row < 9; row++)
            {
                board[row] = new int[9];

                for (int col = 0; col < 9; col++)
                {
                    board[row][col] =
                        CurrentSudoku.Game.Board[row, col];
                }
            }

            return board;
        }

        private bool IsValidCell(int row, int col)
        {
            return row >= 0 && row < 9
                && col >= 0 && col < 9;
        }

        // GET: /Home/Privacy
        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        // Error
        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id
                    ?? HttpContext.TraceIdentifier
            });
        }
    }
}
