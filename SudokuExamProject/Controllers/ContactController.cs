
using Microsoft.AspNetCore.Mvc;
using SudokuExamProject.DataAccessLayer;
using SudokuExamProject.Models;
using SudokuExamProject.Services;
using System;
using System.Threading.Tasks;

namespace SudokuExamProject.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;
        private readonly EmailService _emailService;

        public ContactController(
            AppDbContext context,
            EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new ContactViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 1. Formadan gələn məlumatları obyektə köçürürük
            var contactMessage = new ContactMessage
            {
                Name = model.Name,
                Email = model.Email,
                Subject = model.Subject,
                Message = model.Message,
                CreatedAt = DateTime.Now
            };

            // 2. Mesajı database-ə əlavə edirik
            _context.ContactMessages.Add(contactMessage);

            // 3. Mesajı database-də saxlayırıq
            await _context.SaveChangesAsync();

            try
            {
                // 4. Mesajı email vasitəsilə göndəririk
                await _emailService.SendContactMessageAsync(
                    model.Name,
                    model.Email,
                    model.Subject,
                    model.Message);

                TempData["SuccessMessage"] =
                    "Mesajınız uğurla göndərildi.";
            }
            catch (Exception)
            {
                // Mesaj database-də saxlanılıb,
                // amma email göndərilməsi uğursuz olub
                TempData["WarningMessage"] =
                    "Mesajınız qeydə alındı, lakin email göndərilərkən xəta baş verdi.";
            }

            // 5. Səhifəyə geri qayıdırıq
            return RedirectToAction(nameof(Index));
        }
    }
}
