using Microsoft.EntityFrameworkCore;
using SudokuExamProject.Models;

namespace SudokuExamProject.DataAccessLayer
{
    public class AppDbContext:DbContext

    {
        public AppDbContext(DbContextOptions<AppDbContext> options)  : base(options)
        {
        }
        public DbSet<ContactMessage> ContactMessages { get; set; } 
        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
    }
}
