
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SudokuExamProject.ViewModels
{
    public class AddUserViewModel
    {
        [Required(ErrorMessage = "Ad daxil edin.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Soyad daxil edin.")]
        public string Surname { get; set; } = string.Empty;

        [Required(ErrorMessage = "İstifadəçi adı daxil edin.")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email daxil edin.")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifrə daxil edin.")]
        public string Password { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "Player";

        public string SudokuLevel { get; set; } = "Beginner";

        public string AccountStatus { get; set; } = "Active";

        public string Note { get; set; } = string.Empty;

        public IFormFile? FormFile { get; set; }
    }
}
