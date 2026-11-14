using System.ComponentModel.DataAnnotations;

namespace SudokuExamProject.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Mövcud parolu daxil edin.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mövcud parol")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yeni parolu daxil edin.")]
        [StringLength(100, MinimumLength = 6,
            ErrorMessage = "Yeni parol ən azı 6 simvoldan ibarət olmalıdır.")]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni parol")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yeni parolu təkrar daxil edin.")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword),
            ErrorMessage = "Yeni parollar uyğun gəlmir.")]
        [Display(Name = "Yeni parolun təkrarı")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}