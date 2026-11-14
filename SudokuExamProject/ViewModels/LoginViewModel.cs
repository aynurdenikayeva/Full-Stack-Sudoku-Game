
using System.ComponentModel.DataAnnotations;

namespace SudokuExamProject.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-poçtu daxil edin.")]
        [EmailAddress(ErrorMessage = "E-poçt formatı düzgün deyil.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifrəni daxil edin.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
