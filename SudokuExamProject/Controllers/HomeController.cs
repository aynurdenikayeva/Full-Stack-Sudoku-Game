
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SudokuExamProject.Models;
using SudokuExamProject.Services;

namespace SudokuExamProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SudokuService _sudokuService;

        public HomeController(
            ILogger<HomeController> logger,
            SudokuService sudokuService)
        {
            _logger = logger;
            _sudokuService = sudokuService;
        }

        // Ana səhifə
        [HttpGet]
        public IActionResult Index()
        {
            _sudokuService.Game.Completed =
                _sudokuService.IsCompleted();

            return View(_sudokuService.Game);
        }

        // Rəqəm daxil etmək
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(int row, int col, int? number)
        {
            if (!IsValidCell(row, col))
            {
                return BadRequest(new
                {
                    message = "Xana düzgün seçilməyib."
                });
            }

            string message =
                _sudokuService.AddNumber(row, col, number);

            return Json(GetGameResult(message, row, col));
        }

        // Yeni oyun və çətinlik seçimi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult New(string difficulty = "Orta")
        {
            _sudokuService.NewGame(difficulty);

            return Redirect(Url.Action(nameof(Index)) + "#oyun");
        }

        // İpucu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Hint(int row, int col)
        {
            if (!IsValidCell(row, col))
            {
                return BadRequest(new
                {
                    message = "Xana düzgün seçilməyib."
                });
            }

            string message = _sudokuService.UseHint(row, col);

            return Json(GetGameResult(message, row, col));
        }

        // Seçilmiş xananı təmizləmək
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
                _sudokuService.ClearCell(row, col);

            return Json(GetGameResult(message, row, col));
        }

        // Son əməliyyatı geri qaytarmaq
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Undo()
        {
            string message = _sudokuService.Undo();

            var game = _sudokuService.Game;
            game.Completed = _sudokuService.IsCompleted();

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

        // Əməliyyatların ümumi cavabı
        private object GetGameResult(
            string message,
            int row,
            int col)
        {
            var game = _sudokuService.Game;

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

        // Lövhəni JavaScript üçün uyğun formata çevirmək
        private int[][] GetBoard()
        {
            var board = new int[9][];

            for (int row = 0; row < 9; row++)
            {
                board[row] = new int[9];

                for (int col = 0; col < 9; col++)
                {
                    board[row][col] =
                        _sudokuService.Game.Board[row, col];
                }
            }

            return board;
        }

        // Xana indekslərini yoxlamaq
        private bool IsValidCell(int row, int col)
        {
            return row >= 0 && row < 9
                && col >= 0 && col < 9;
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

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
