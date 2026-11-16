
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace SudokuExamProject.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;

        public string Surname { get; set; } = string.Empty;

        public string SudokuLevel { get; set; } = "Beginner";

        public string AccountStatus { get; set; } = "Active";

        public string Note { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        public bool IsOnline { get; set; } = false;


        [NotMapped]
        public IFormFile? FormFile { get; set; }
    }
}
