using SudokuExamProject.Models;
using System.Collections.Generic;

namespace SudokuExamProject.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalUsers { get; set; }

        public int AdminCount { get; set; }

        public int PlayerCount { get; set; }

        public int ActiveUsers { get; set; }

        public int InactiveUsers { get; set; }

        public int BlockedUsers { get; set; }

        public int TotalMessages { get; set; }

        public List<AppUser> Users { get; set; } = new();

        // Admin faizi
        public double AdminPercentage =>
            TotalUsers == 0 ? 0 :
            (double)AdminCount / TotalUsers * 100;

        // Player faizi
        public double PlayerPercentage =>
            TotalUsers == 0 ? 0 :
            (double)PlayerCount / TotalUsers * 100;

        // Aktiv istifadəçilərin faizi
        public double ActivePercentage =>
            TotalUsers == 0 ? 0 :
            (double)ActiveUsers / TotalUsers * 100;

        // Deaktiv istifadəçilərin faizi
        public double InactivePercentage =>
            TotalUsers == 0 ? 0 :
            (double)InactiveUsers / TotalUsers * 100;

        // Bloklanmış istifadəçilərin faizi
        public double BlockedPercentage =>
            TotalUsers == 0 ? 0 :
            (double)BlockedUsers / TotalUsers * 100;
    }
}
