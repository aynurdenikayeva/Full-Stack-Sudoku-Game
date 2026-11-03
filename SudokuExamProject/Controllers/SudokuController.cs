
using Microsoft.AspNetCore.Mvc;
using SudokuExamProject.Services;

namespace SudokuExamProject.Controllers
{
    public class SudokuController : Controller
    {
        private readonly SudokuService _sudokuService;

        public SudokuController(SudokuService sudokuService)
        {
            _sudokuService = sudokuService;
        }

        // GET: /Sudoku
        [HttpGet]
        public IActionResult Index()
        {
            _sudokuService.Game.Completed =
                _sudokuService.IsCompleted();

            return View(_sudokuService.Game);
        }

        // POST: /Sudoku/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(
            int row,
            int col,
            int? number)
        {
            if (!IsValidCell(row, col))
            {
                return BadRequest("Xana düzgün seçilməyib.");
            }

            _sudokuService.Game.Message =
                _sudokuService.AddNumber(row, col, number);

            _sudokuService.Game.Completed =
                _sudokuService.IsCompleted();

            return RedirectToAction(nameof(Index));
        }

        // POST: /Sudoku/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult New(string difficulty = "Orta")
        {
            _sudokuService.NewGame(difficulty);

            return RedirectToAction(nameof(Index));
        }

        // POST: /Sudoku/Hint
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Hint(int row, int col)
        {
            if (!IsValidCell(row, col))
            {
                return BadRequest("Xana düzgün seçilməyib.");
            }

            _sudokuService.Game.Message =
                _sudokuService.UseHint(row, col);

            return RedirectToAction(nameof(Index));
        }

        // POST: /Sudoku/Clear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Clear(int row, int col)
        {
            if (!IsValidCell(row, col))
            {
                return BadRequest("Xana düzgün seçilməyib.");
            }

            _sudokuService.Game.Message =
                _sudokuService.ClearCell(row, col);

            return RedirectToAction(nameof(Index));
        }

        // POST: /Sudoku/Undo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Undo()
        {
            _sudokuService.Game.Message =
                _sudokuService.Undo();

            _sudokuService.Game.Completed =
                _sudokuService.IsCompleted();

            return RedirectToAction(nameof(Index));
        }

        private bool IsValidCell(int row, int col)
        {
            return row >= 0 && row < 9
                && col >= 0 && col < 9;
        }
    }
}
