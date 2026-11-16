
using System.ComponentModel.DataAnnotations;

namespace SudokuExamProject.Models
{
    public class SiteSettings
    {
        public int Id { get; set; } = 1;

        [Required(ErrorMessage = "Saytın adı boş ola bilməz.")]
        [StringLength(100)]
        public string SiteName { get; set; } = "AXIOMIX";

        [StringLength(500)]
        public string? SiteDescription { get; set; }

        [EmailAddress(ErrorMessage = "Email düzgün deyil.")]
        public string? ContactEmail { get; set; }

        public bool IsGameEnabled { get; set; } = true;

        public bool AllowRegistration { get; set; } = true;

        public bool AllowHints { get; set; } = true;

        public string DefaultDifficulty { get; set; } = "Orta";

        [Range(6, 32)]
        public int MinimumPasswordLength { get; set; } = 8;

        [Range(1, 10)]
        public int MaxFailedLoginAttempts { get; set; } = 5;
    }
}
