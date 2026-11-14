using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SudokuExamProject.Models
{
    public class SudokuResult
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey(nameof(UserId))]
        public AppUser? User { get; set; }

        public int ElapsedSeconds { get; set; }

        public int Mistakes { get; set; }

        public int HintsUsed { get; set; }

        [Required]
        public string Difficulty { get; set; } = "Orta";

        public DateTime CompletedAt { get; set; } = DateTime.UtcNow;
    }
}