
using System.Collections.Generic;

namespace SudokuExamProject.Models
{
    public class SudokuGame
    {
        // Sudoku lövhəsi: 0 boş xana deməkdir.
        public int[,] Board { get; set; } = new int[9, 9];

        // Sudoku tapmacasının tam həlli.
        public int[,] Solution { get; set; } = new int[9, 9];

        // Əvvəlcədən verilmiş rəqəmlər dəyişdirilə bilməz.
        public bool[,] Fixed { get; set; } = new bool[9, 9];

        // Seçilmiş çətinlik səviyyəsi.
        public string Difficulty { get; set; } = "Orta";

        // İstifadəçinin etdiyi səhvlərin sayı.
        public int Mistakes { get; set; } = 0;

        // İstifadə olunan ipuclarının sayı.
        public int HintsUsed { get; set; } = 0;

        // Oyunun tamamlanma və məğlubiyyət vəziyyəti.
        public bool Completed { get; set; } = false;
        public bool Lost { get; set; } = false;

        // İcazə verilən maksimum limitlər.
        public const int MaxMistakes = 5;
        public const int MaxHints = 3;

        // İstifadəçiyə göstərilən mesaj.
        public string Message { get; set; } = string.Empty;

        // Əməliyyatları geri qaytarmaq üçün tarixçə.
        public List<SudokuMove> UndoHistory { get; set; }
            = new List<SudokuMove>();
    }

    // Sudoku lövhəsində edilmiş bir əməliyyat.
    public class SudokuMove
    {
        public int Row { get; set; }

        public int Col { get; set; }

        // Əməliyyatdan əvvəl xanada olan rəqəm.
        public int PreviousValue { get; set; }

        // Əməliyyatdan sonra xanaya yazılan rəqəm.
        public int NewValue { get; set; }
    }
}
