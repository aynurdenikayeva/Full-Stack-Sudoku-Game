using Microsoft.AspNetCore.Identity;

namespace SudokuExamProject.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
    }
}
