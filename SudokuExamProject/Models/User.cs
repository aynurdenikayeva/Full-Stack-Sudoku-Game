using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SudokuExamProject.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad daxil edin.")]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Soyad daxil edin.")]
        [StringLength(50)]
        public string Surname { get; set; } = string.Empty;

        [Required(ErrorMessage = "İstifadəçi adı daxil edin.")]
        [StringLength(50)]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email ünvanınızı daxil edin.")]
        [EmailAddress(ErrorMessage = "Düzgün email ünvanı daxil edin.")]
        [Display(Name = "Email ünvanı")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Düzgün telefon nömrəsi daxil edin.")]
        public string TelephonNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "İstifadəçi rolunu seçin.")]
        public string UserRole { get; set; } = "Player";

        [Required(ErrorMessage = "Sudoku səviyyəsini seçin.")]
        public string SudokuLevel { get; set; } = "Beginner";

        [Required(ErrorMessage = "Hesab statusunu seçin.")]
        public string AccountStatus { get; set; } = "Active";

        [StringLength(500)]
        public string Note { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        [NotMapped]
        public IFormFile formFile { get; set; }

        public string Image { get; set; }


    }
}
