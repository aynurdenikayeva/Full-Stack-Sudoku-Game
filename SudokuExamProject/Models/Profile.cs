using System.ComponentModel.DataAnnotations;

namespace SudokuExamProject.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; } 
        public string UserName { get; set; }

        public string Email { get; set; }
        public string TelephonNumber { get; set; }
        public string Note { get; set; }

        public string Image { get; set; }


    }
}
