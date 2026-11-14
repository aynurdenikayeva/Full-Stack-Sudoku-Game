
using System;
using System.Collections.Generic;

namespace SudokuExamProject.ViewModels
{
    public class LeaderboardViewModel
    {
        public string SelectedDifficulty { get; set; } = "Asan";

        public List<string> Difficulties { get; set; } =
            new List<string>
            {
                "Hamısı",
                "Asan",
                "Orta",
                "Çətin",
                "Ekspert",
                "Usta",
                "Ekstremal"
            };

        public List<LeaderboardEntryViewModel> Entries { get; set; }
            = new List<LeaderboardEntryViewModel>();

        public int? CurrentUserRank { get; set; }

        public LeaderboardEntryViewModel? CurrentUserEntry { get; set; }
    }

    public class LeaderboardEntryViewModel
    {
        public int Rank { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string PlayerName { get; set; } = string.Empty;

        public int GamesPlayed { get; set; }

        public int BestTimeSeconds { get; set; }

        public double AverageTimeSeconds { get; set; }

        public int TotalMistakes { get; set; }

        public int BestRunMistakes { get; set; }

        public DateTime LastPlayedAt { get; set; }

        public bool IsCurrentUser { get; set; }
    }
}
