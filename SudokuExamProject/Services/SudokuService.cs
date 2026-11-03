
using System;
using System.Collections.Generic;
using SudokuExamProject.Models;

namespace SudokuExamProject.Services
{
    public class SudokuService
    {
        private readonly Random random = new Random();
        private readonly Stack<SudokuMove> undoStack =
            new Stack<SudokuMove>();

        public SudokuGame Game { get; private set; } = new SudokuGame();

        public SudokuService()
        {
            NewGame("Orta");
        }

        // Yeni oyun yaradır və bütün sayğacları sıfırlayır.
        public void NewGame(string difficulty = "Orta")
        {
            int[,] solution = GenerateCompleteBoard();

            Game = new SudokuGame
            {
                Solution = solution,
                Difficulty = NormalizeDifficulty(difficulty),
                Mistakes = 0,
                HintsUsed = 0,
                Completed = false,
                Lost = false,
                Message = string.Empty
            };

            Array.Copy(solution, Game.Board, solution.Length);

            RemoveNumbers(Game.Difficulty);

            undoStack.Clear();
        }

        // Çətinlik səviyyəsini müəyyən edir.
        private string NormalizeDifficulty(string difficulty)
        {
            string[] levels =
            {
                "Asan", "Orta", "Çətin",
                "Ekspert", "Usta", "Ekstremal"
            };

            foreach (string level in levels)
            {
                if (string.Equals(
                    level,
                    difficulty,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return level;
                }
            }

            return "Orta";
        }

        // Tam Sudoku həlli yaradır.
        private int[,] GenerateCompleteBoard()
        {
            int[,] board = new int[9, 9];

            FillBoard(board);

            return board;
        }

        // Backtracking ilə lövhəni doldurur.
        private bool FillBoard(int[,] board)
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (board[row, col] != 0)
                        continue;

                    int[] numbers = GetShuffledNumbers();

                    foreach (int number in numbers)
                    {
                        if (!IsValid(board, row, col, number))
                            continue;

                        board[row, col] = number;

                        if (FillBoard(board))
                            return true;

                        board[row, col] = 0;
                    }

                    return false;
                }
            }

            return true;
        }

        // 1-dən 9-a qədər rəqəmləri qarışdırır.
        private int[] GetShuffledNumbers()
        {
            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            for (int i = numbers.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);

                (numbers[i], numbers[j]) =
                    (numbers[j], numbers[i]);
            }

            return numbers;
        }

        // Rəqəmin sətir, sütun və 3x3 blok üçün uyğunluğunu yoxlayır.
        private bool IsValid(
            int[,] board,
            int row,
            int col,
            int number)
        {
            for (int i = 0; i < 9; i++)
            {
                if (board[row, i] == number ||
                    board[i, col] == number)
                {
                    return false;
                }
            }

            int startRow = row - row % 3;
            int startCol = col - col % 3;

            for (int i = startRow; i < startRow + 3; i++)
            {
                for (int j = startCol; j < startCol + 3; j++)
                {
                    if (board[i, j] == number)
                        return false;
                }
            }

            return true;
        }

        // Çətinliyə uyğun xanaları boşaldır.
        // Hər silmədən sonra tapmacanın unikal həllini yoxlayır.
        private void RemoveNumbers(string difficulty)
        {
            int cellsToRemove = difficulty switch
            {
                "Asan" => 35,
                "Orta" => 45,
                "Çətin" => 52,
                "Ekspert" => 57,
                "Usta" => 61,
                "Ekstremal" => 64,
                _ => 45
            };

            // Başlanğıcda bütün xanalar sabitdir.
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    Game.Fixed[row, col] = true;
                }
            }

            // Bütün xana koordinatlarını siyahıya əlavə edirik.
            List<(int Row, int Col)> cells =
                new List<(int Row, int Col)>();

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    cells.Add((row, col));
                }
            }

            // Xanaların sırasını qarışdırırıq.
            for (int i = cells.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);

                (cells[i], cells[j]) =
                    (cells[j], cells[i]);
            }

            int removed = 0;

            foreach (var cell in cells)
            {
                if (removed >= cellsToRemove)
                    break;

                int row = cell.Row;
                int col = cell.Col;

                int previousValue = Game.Board[row, col];

                // Rəqəmi müvəqqəti silirik.
                Game.Board[row, col] = 0;

                // Maksimum iki həll axtarırıq.
                int solutionCount = CountSolutions(Game.Board, 2);

                if (solutionCount == 1)
                {
                    // Yalnız bir həll varsa, xananı boş saxlayırıq.
                    Game.Fixed[row, col] = false;
                    removed++;
                }
                else
                {
                    // Unikal həll pozulursa, rəqəmi qaytarırıq.
                    Game.Board[row, col] = previousValue;
                }
            }
        }

        // Tapmacanın həllərinin sayını hesablayır.
        // limit = 2 olduqda iki həll tapmaq kifayətdir.
        private int CountSolutions(int[,] board, int limit)
        {
            int count = 0;

            CountSolutionsRecursive(board, limit, ref count);

            return count;
        }

        private void CountSolutionsRecursive(
            int[,] board,
            int limit,
            ref int count)
        {
            if (count >= limit)
                return;

            int emptyRow = -1;
            int emptyCol = -1;
            bool foundEmpty = false;

            // İlk boş xananı tapırıq.
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (board[row, col] == 0)
                    {
                        emptyRow = row;
                        emptyCol = col;
                        foundEmpty = true;
                        break;
                    }
                }

                if (foundEmpty)
                    break;
            }

            // Boş xana yoxdursa, bir həll tapılıb.
            if (!foundEmpty)
            {
                count++;
                return;
            }

            // Boş xanaya mümkün rəqəmləri yerləşdiririk.
            for (int number = 1; number <= 9; number++)
            {
                if (!IsValid(board, emptyRow, emptyCol, number))
                    continue;

                board[emptyRow, emptyCol] = number;

                CountSolutionsRecursive(board, limit, ref count);

                // Lövhəni əvvəlki vəziyyətinə qaytarırıq.
                board[emptyRow, emptyCol] = 0;

                if (count >= limit)
                    return;
            }
        }

        // Rəqəm daxil edir və səhvləri sayır.
        public string AddNumber(int row, int col, int? number)
        {
            if (!IsValidCell(row, col))
                return "Xana düzgün seçilməyib.";

            if (IsGameOver())
                return GetGameOverMessage();

            if (Game.Fixed[row, col])
                return "Bu xana dəyişdirilə bilməz.";

            if (!number.HasValue ||
                number.Value < 1 ||
                number.Value > 9)
            {
                return "1-dən 9-a qədər rəqəm daxil edin.";
            }

            if (Game.Board[row, col] == number.Value)
                return "Bu rəqəm artıq xanada yerləşir.";

            // Səhv cavab daxil edilibsə.
            if (number.Value != Game.Solution[row, col])
            {
                Game.Mistakes++;

                if (Game.Mistakes >= SudokuGame.MaxMistakes)
                {
                    Game.Lost = true;
                    Game.Message =
                        "5 səhv etdin! Oyunu uduzdun. Yeni oyun başlat.";

                    return Game.Message;
                }

                Game.Message =
                    $"Səhv cavab! Qalan səhv haqqın: " +
                    $"{SudokuGame.MaxMistakes - Game.Mistakes}";

                return Game.Message;
            }

            // Düzgün cavabı lövhəyə yazırıq.
            SaveMove(
                row,
                col,
                Game.Board[row, col],
                number.Value);

            Game.Board[row, col] = number.Value;
            Game.Message = "Düzgün rəqəm!";

            CheckCompletion();

            return Game.Message;
        }

        // Seçilmiş xanaya ipucu verir.
        public string UseHint(int row, int col)
        {
            if (!IsValidCell(row, col))
                return "Xana düzgün seçilməyib.";

            if (IsGameOver())
                return GetGameOverMessage();

            if (Game.HintsUsed >= SudokuGame.MaxHints)
            {
                Game.Message =
                    "Maksimum 3 ipucu istifadə edə bilərsən!";

                return Game.Message;
            }

            if (Game.Fixed[row, col])
                return "Əvvəlcədən verilmiş xana seçilib.";

            if (Game.Board[row, col] == Game.Solution[row, col])
                return "Bu xana artıq düzgündür.";

            SaveMove(
                row,
                col,
                Game.Board[row, col],
                Game.Solution[row, col]);

            Game.Board[row, col] = Game.Solution[row, col];
            Game.HintsUsed++;

            Game.Message =
                $"İpucu istifadə edildi. Qalan ipucu: " +
                $"{SudokuGame.MaxHints - Game.HintsUsed}";

            CheckCompletion();

            return Game.Message;
        }

        // Seçilmiş xanadakı rəqəmi silir.
        public string ClearCell(int row, int col)
        {
            if (!IsValidCell(row, col))
                return "Xana düzgün seçilməyib.";

            if (IsGameOver())
                return GetGameOverMessage();

            if (Game.Fixed[row, col])
                return "Bu xana dəyişdirilə bilməz.";

            if (Game.Board[row, col] == 0)
                return "Xana artıq boşdur.";

            SaveMove(
                row,
                col,
                Game.Board[row, col],
                0);

            Game.Board[row, col] = 0;
            Game.Message = "Xana təmizləndi.";

            return Game.Message;
        }

        // Son əməliyyatı geri qaytarır.
        public string Undo()
        {
            if (IsGameOver())
                return GetGameOverMessage();

            if (undoStack.Count == 0)
                return "Geri qaytarılacaq əməliyyat yoxdur.";

            SudokuMove move = undoStack.Pop();

            Game.Board[move.Row, move.Col] = move.PreviousValue;
            Game.Message = "Son əməliyyat geri qaytarıldı.";

            return Game.Message;
        }

        // Əməliyyatı geri qaytarmaq üçün saxlayır.
        private void SaveMove(
            int row,
            int col,
            int previousValue,
            int newValue)
        {
            undoStack.Push(new SudokuMove
            {
                Row = row,
                Col = col,
                PreviousValue = previousValue,
                NewValue = newValue
            });
        }

        // Koordinatların düzgünlüyünü yoxlayır.
        private bool IsValidCell(int row, int col)
        {
            return row >= 0 && row < 9 &&
                   col >= 0 && col < 9;
        }

        // Oyunun bitib-bitmədiyini yoxlayır.
        private bool IsGameOver()
        {
            return Game.Completed || Game.Lost;
        }

        // Oyun bitdikdə uyğun mesaj qaytarır.
        private string GetGameOverMessage()
        {
            if (Game.Lost)
                return "Oyunu uduzmusan! Yeni oyun başlat.";

            return "Təbriklər! Oyunu artıq tamamlamısan.";
        }

        // Lövhənin tamamlanmasını yoxlayır.
        private void CheckCompletion()
        {
            if (IsCompleted())
            {
                Game.Completed = true;
                Game.Message = "Təbriklər! Sudoku tamamlandı!";
            }
        }

        // Bütün xanaların düzgün doldurulmasını yoxlayır.
        public bool IsCompleted()
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    if (Game.Board[row, col] != Game.Solution[row, col])
                        return false;
                }
            }

            return true;
        }

    }
}
