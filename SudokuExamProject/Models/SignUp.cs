using System.ComponentModel.DataAnnotations;

namespace SudokuExamProject.Models
{
    public class SignUp
    {
        [Required(ErrorMessage = "Adınızı daxil edin.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-poçtu daxil edin.")]
        [EmailAddress(ErrorMessage = "E-poçt formatı düzgün deyil.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifrəni daxil edin.")]
        [MinLength(8, ErrorMessage = "Şifrə ən azı 8 simvol olmalıdır.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifrəni təkrar daxil edin.")]
        [Compare("Password", ErrorMessage = "Şifrələr eyni deyil.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
        public string TelephonNumber { get; set; } = string.Empty;
    }
}
