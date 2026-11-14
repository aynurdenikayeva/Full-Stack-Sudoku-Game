using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SudokuExamProject.Models;

namespace SudokuExamProject.DataAccessLayer
{
    public class AppDbContext: IdentityDbContext<AppUser>

    {
        public AppDbContext(DbContextOptions<AppDbContext> options)  : base(options)
        {
        }
        public DbSet<ContactMessage> ContactMessages { get; set; } 
        public DbSet<Profile> Profiles { get; set; }

        public DbSet<SudokuResult> SudokuResults { get; set; }

    }
}
