
using System.ComponentModel.DataAnnotations;

namespace SudokuExamProject.ViewModels
{
    public class UpdateUserViewModel
    {
        [Required]
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ad daxil edilməlidir.")]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Soyad daxil edilməlidir.")]
        [StringLength(50)]
        public string Surname { get; set; } = string.Empty;

        [Required(ErrorMessage = "İstifadəçi adı daxil edilməlidir.")]
        [StringLength(30)]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-poçt daxil edilməlidir.")]
        [EmailAddress(ErrorMessage = "Düzgün e-poçt ünvanı daxil edin.")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Telefon nömrəsi düzgün deyil.")]
        public string? PhoneNumber { get; set; }

        [Required]
        public string Role { get; set; } = "Player";

        public string SudokuLevel { get; set; } = "Beginner";

        public string AccountStatus { get; set; } = "Active";

        [StringLength(1000)]
        public string? Note { get; set; }
    }
}
