using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SudokuExamProject.Models;
using SudokuExamProject.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SudokuExamProject.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public ProfileController(
            UserManager<AppUser> userManager,
            IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _environment = environment;
        }

        // Profilə baxış
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Challenge();

            return View(user);
        }

        // Profil redaktə səhifəsi
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Challenge();

            var model = new EditProfileViewModel
            {
                Username = user.UserName ?? string.Empty,
                Name = user.Name,
                Surname = user.Surname,
                PhoneNumber = user.PhoneNumber,
                CurrentImage = user.Image
            };

            return View(model);
        }

        // Profil məlumatlarını yadda saxlamaq
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Challenge();

            model.CurrentImage = user.Image;

            if (!ModelState.IsValid)
                return View(model);

            string? newImagePath = null;
            // İstifadəçi adını yoxla və yenilə
            var newUsername = model.Username.Trim();

            var existingUser = await _userManager.FindByNameAsync(newUsername);

            if (existingUser != null && existingUser.Id != user.Id)
            {
                ModelState.AddModelError(
                    nameof(model.Username),
                    "Bu istifadəçi adı artıq istifadə olunur.");

                return View(model);
            }

            var usernameResult = await _userManager.SetUserNameAsync(
                user,
                newUsername);

            if (!usernameResult.Succeeded)
            {
                foreach (var error in usernameResult.Errors)
                {
                    ModelState.AddModelError(
                        nameof(model.Username),
                        error.Description);
                }

                return View(model);
            }
            // Şəkil yüklənibsə
            if (model.ProfileImage != null &&
                model.ProfileImage.Length > 0)
            {
                var extension = Path.GetExtension(
                    model.ProfileImage.FileName).ToLowerInvariant();

                var allowedExtensions = new[]
                {
                    ".jpg", ".jpeg", ".png", ".webp"
                };

                if (!Array.Exists(allowedExtensions,
                    x => x == extension))
                {
                    ModelState.AddModelError(
                        nameof(model.ProfileImage),
                        "Yalnız JPG, JPEG, PNG və WEBP formatlı şəkillər yükləyə bilərsiniz.");

                    return View(model);
                }

                if (model.ProfileImage.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError(
                        nameof(model.ProfileImage),
                        "Şəklin ölçüsü maksimum 2 MB olmalıdır.");

                    return View(model);
                }

                var uploadsFolder = Path.Combine(
                    _environment.WebRootPath,
                    "uploads",
                    "profiles");

                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString("N") + extension;

                var filePath = Path.Combine(
                    uploadsFolder,
                    fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfileImage.CopyToAsync(stream);
                }

                newImagePath = "/uploads/profiles/" + fileName;
            }

            // İstifadəçi məlumatlarını yenilə
            user.Name = model.Name.Trim();
            user.Surname = model.Surname.Trim();
            user.PhoneNumber = model.PhoneNumber?.Trim();

            if (newImagePath != null)
            {
                user.Image = newImagePath;
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                // Yenilənmə uğursuz olarsa yeni şəkli sil
                if (newImagePath != null)
                {
                    var newFilePath = Path.Combine(
                        _environment.WebRootPath,
                        newImagePath.TrimStart('/')
                            .Replace('/', Path.DirectorySeparatorChar));

                    if (System.IO.File.Exists(newFilePath))
                        System.IO.File.Delete(newFilePath);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);
                }

                return View(model);
            }

            // Yeni şəkil uğurla yadda saxlanıldıqdan sonra
            // köhnə yüklənmiş profil şəklini sil
            if (newImagePath != null &&
                !string.IsNullOrWhiteSpace(model.CurrentImage) &&
                model.CurrentImage.StartsWith(
                    "/uploads/profiles/",
                    StringComparison.OrdinalIgnoreCase))
            {
                var oldFilePath = Path.Combine(
                    _environment.WebRootPath,
                    model.CurrentImage.TrimStart('/')
                        .Replace('/', Path.DirectorySeparatorChar));

                if (System.IO.File.Exists(oldFilePath))
                    System.IO.File.Delete(oldFilePath);
            }

            TempData["Success"] = "Profil məlumatlarınız uğurla yeniləndi.";

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Challenge();

            // Model yoxlanışı
            if (!ModelState.IsValid)
            {
                TempData["PasswordError"] = "Daxil etdiyiniz məlumatları düzgün yoxlayın.";
                return RedirectToAction(nameof(Edit));
            }

            // Yeni parolların uyğunluğunu yoxla
            if (model.NewPassword != model.ConfirmNewPassword)
            {
                TempData["PasswordError"] = "Yeni parollar uyğun gəlmir.";
                return RedirectToAction(nameof(Edit));
            }

            // Köhnə parolu yoxla və yeni parolu yadda saxla
            var result = await _userManager.ChangePasswordAsync(
                user,
                model.CurrentPassword,
                model.NewPassword
            );

            if (result.Succeeded)
            {
                TempData["PasswordSuccess"] = "Parolunuz uğurla dəyişdirildi.";

                return RedirectToAction(nameof(Edit));
            }

            // Identity xətalarını Azərbaycan dilində göstər
            var errors = result.Errors.Select(error => error.Code switch
            {
                "PasswordMismatch" =>
                    "Mövcud parol düzgün deyil.",

                "PasswordTooShort" =>
                    "Yeni parol ən azı 6 simvoldan ibarət olmalıdır.",

                "PasswordRequiresDigit" =>
                    "Yeni parolda ən azı bir rəqəm olmalıdır.",

                "PasswordRequiresUpper" =>
                    "Yeni parolda ən azı bir böyük hərf olmalıdır.",

                "PasswordRequiresLower" =>
                    "Yeni parolda ən azı bir kiçik hərf olmalıdır.",

                "PasswordRequiresNonAlphanumeric" =>
                    "Yeni parolda ən azı bir xüsusi simvol olmalıdır.",

                _ => error.Description
            });

            TempData["PasswordError"] = string.Join(" ", errors);

            return RedirectToAction(nameof(Edit));
        }
    }
}
