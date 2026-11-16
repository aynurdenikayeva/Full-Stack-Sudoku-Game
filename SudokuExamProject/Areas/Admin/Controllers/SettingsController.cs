
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SudokuExamProject.DataAccessLayer;
using SudokuExamProject.Models;
using System.Threading.Tasks;

namespace SudokuExamProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SettingsController : Controller
    {
        private readonly AppDbContext _context;

        public SettingsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var settings = await _context.SiteSettings
                .FirstOrDefaultAsync(x => x.Id == 1);

            if (settings == null)
            {
                settings = new SiteSettings { Id = 1 };

                _context.SiteSettings.Add(settings);
                await _context.SaveChangesAsync();
            }

            return View(settings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(SiteSettings model)
        {
            if (model.Id != 1)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var settings = await _context.SiteSettings
                .FirstOrDefaultAsync(x => x.Id == 1);

            if (settings == null)
            {
                settings = new SiteSettings { Id = 1 };
                _context.SiteSettings.Add(settings);
            }

            // Yalnız təhlükəsizlik parametrləri yenilənir
            settings.AllowRegistration = model.AllowRegistration;
            settings.MinimumPasswordLength = model.MinimumPasswordLength;
            settings.MaxFailedLoginAttempts = model.MaxFailedLoginAttempts;

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "Təhlükəsizlik parametrləri uğurla yadda saxlanıldı.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reset()
        {
            var settings = await _context.SiteSettings
                .FirstOrDefaultAsync(x => x.Id == 1);

            if (settings == null)
            {
                settings = new SiteSettings { Id = 1 };
                _context.SiteSettings.Add(settings);
            }

            // Standart təhlükəsizlik parametrləri
            settings.AllowRegistration = true;
            settings.MinimumPasswordLength = 8;
            settings.MaxFailedLoginAttempts = 5;

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "Təhlükəsizlik parametrləri standart vəziyyətə qaytarıldı.";

            return RedirectToAction(nameof(Index));
        }
    }
}
