
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SudokuExamProject.ViewModels
{
    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "Adınızı daxil edin.")]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Soyadınızı daxil edin.")]
        [StringLength(50)]
        public string Surname { get; set; } = string.Empty;
        [Required(ErrorMessage = "İstifadəçi adı boş ola bilməz.")]
        [StringLength(50, ErrorMessage = "İstifadəçi adı maksimum 50 simvol ola bilər.")]
        public string Username { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Telefon nömrəsi düzgün deyil.")]
        public string? PhoneNumber { get; set; }

        public IFormFile? ProfileImage { get; set; }

        public string? CurrentImage { get; set; }

    }
}
