using System.ComponentModel.DataAnnotations;

namespace SudokuExamProject.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Adınızı daxil edin.")]
        [Display(Name = "Adınız")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email ünvanınızı daxil edin.")]
        [EmailAddress(ErrorMessage = "Düzgün email ünvanı daxil edin.")]
        [Display(Name = "Email ünvanı")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mövzunu daxil edin.")]
        [Display(Name = "Mövzu")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mesajınızı daxil edin.")]
        [Display(Name = "Mesaj")]
        public string Message { get; set; } = string.Empty;
    }
}
