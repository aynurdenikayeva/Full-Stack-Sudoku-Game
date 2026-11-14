
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SudokuExamProject.Services;

namespace SudokuExamProject.Controllers
{
    public class SudokuController : Controller
    {
        private readonly SudokuGameManager _gameManager;

        public SudokuController(SudokuGameManager gameManager)
        {
            _gameManager = gameManager;
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

        // GET: /Sudoku
        [HttpGet]
        public IActionResult Index()
        {
            CurrentSudoku.Game.Completed =
                CurrentSudoku.IsCompleted();

            return View(CurrentSudoku.Game);
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

            CurrentSudoku.Game.Message =
                CurrentSudoku.AddNumber(row, col, number);

            CurrentSudoku.Game.Completed =
                CurrentSudoku.IsCompleted();

            return RedirectToAction(nameof(Index));
        }

        // POST: /Sudoku/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult New(string difficulty = "Orta")
        {
            CurrentSudoku.NewGame(difficulty);

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

            CurrentSudoku.Game.Message =
                CurrentSudoku.UseHint(row, col);

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

            CurrentSudoku.Game.Message =
                CurrentSudoku.ClearCell(row, col);

            return RedirectToAction(nameof(Index));
        }

        // POST: /Sudoku/Undo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Undo()
        {
            CurrentSudoku.Game.Message =
                CurrentSudoku.Undo();

            CurrentSudoku.Game.Completed =
                CurrentSudoku.IsCompleted();

            return RedirectToAction(nameof(Index));
        }

        private bool IsValidCell(int row, int col)
        {
            return row >= 0 && row < 9
                && col >= 0 && col < 9;
        }
    }
}
