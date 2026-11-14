
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SudokuExamProject.Models;
using SudokuExamProject.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SudokuExamProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UsersController(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _webHostEnvironment = webHostEnvironment;
        }

        // Bütün istifadəçilər
        public async Task<IActionResult> Index()
        {
            List<AppUser> users = await _userManager.Users.ToListAsync();

            var userRoles = new Dictionary<string, IList<string>>();

            foreach (var user in users)
            {
                userRoles[user.Id] = await _userManager.GetRolesAsync(user);
            }

            ViewBag.UserRoles = userRoles;

            return View(users);
        }

        // İstifadəçi əlavə etmək - GET
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        // İstifadəçi əlavə etmək - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddUserViewModel model)
        {
            await EnsureRolesExist();
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Role != "Admin" && model.Role != "Player")
            {
                ModelState.AddModelError("Role", "Yanlış rol seçilib.");
                return View(model);
            }

            if (!await _roleManager.RoleExistsAsync(model.Role))
            {
                ModelState.AddModelError("Role", "Seçilmiş rol sistemdə mövcud deyil.");
                return View(model);
            }

            var user = new AppUser
            {
                Name = model.Name.Trim(),
                Surname = model.Surname.Trim(),
                UserName = model.UserName.Trim(),
                Email = model.Email.Trim(),
                PhoneNumber = model.PhoneNumber,
                SudokuLevel = model.SudokuLevel,
                AccountStatus = model.AccountStatus,
                Note = model.Note
            };

            // Şəkil yüklənməsi
            if (model.FormFile != null && model.FormFile.Length > 0)
            {
                string uploadFolder = Path.Combine(
                    _webHostEnvironment.WebRootPath,
                    "uploads");

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                string fileName = Guid.NewGuid().ToString() +
                    Path.GetExtension(Path.GetFileName(model.FormFile.FileName));

                string filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.FormFile.CopyToAsync(stream);
                }

                user.Image = "/uploads/" + fileName;
            }

            // Identity vasitəsilə istifadəçi yaradılır.
            // Şifrə avtomatik hash edilir.
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            // İstifadəçiyə rol təyin edilir.
            var roleResult = await _userManager.AddToRoleAsync(user, model.Role);

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);

                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }
        private async Task EnsureRolesExist()
        {
            string[] roles = { "Admin", "Player" };

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    var result = await _roleManager.CreateAsync(
                        new IdentityRole(role));

                    if (!result.Succeeded)
                    {
                        throw new Exception(
                            $"'{role}' rolu yaradılmadı: " +
                            string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
            }
        }
        // İstifadəçi məlumatları - GET
        [HttpGet]
        public async Task<IActionResult> Read(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            AppUser user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            ViewBag.UserRoles = await _userManager.GetRolesAsync(user);

            return View(user);
        }

        // İstifadəçini redaktə etmək - GET
        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            AppUser user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            var model = new UpdateUserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber,
                Role = roles.FirstOrDefault() ?? "Player",
                SudokuLevel = user.SudokuLevel,
                AccountStatus = user.AccountStatus,
                Note = user.Note
            };

            return View(model);
        }

        // İstifadəçini redaktə etmək - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Role != "Admin" && model.Role != "Player")
            {
                ModelState.AddModelError("Role", "Yanlış rol seçilib.");
                return View(model);
            }

            AppUser user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                return NotFound();
            }

            if (!await _roleManager.RoleExistsAsync(model.Role))
            {
                ModelState.AddModelError("Role", "Seçilmiş rol sistemdə mövcud deyil.");
                return View(model);
            }

            user.Name = model.Name.Trim();
            user.Surname = model.Surname.Trim();
            user.UserName = model.UserName.Trim();
            user.Email = model.Email.Trim();
            user.PhoneNumber = model.PhoneNumber;
            user.SudokuLevel = model.SudokuLevel;
            user.AccountStatus = model.AccountStatus;
            user.Note = model.Note ?? string.Empty;

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            // Köhnə rollar silinir, yeni rol təyin edilir.
            var oldRoles = await _userManager.GetRolesAsync(user);

            if (oldRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, oldRoles);

                if (!removeResult.Succeeded)
                {
                    foreach (var error in removeResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return View(model);
                }
            }

            var addRoleResult = await _userManager.AddToRoleAsync(user, model.Role);

            if (!addRoleResult.Succeeded)
            {
                foreach (var error in addRoleResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // İstifadəçini silmək - GET
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            AppUser user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // İstifadəçini silmək - POST
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            AppUser user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            // Admin öz hesabını silə bilməz.
            if (user.Id == _userManager.GetUserId(User))
            {
                TempData["Error"] = "Daxil olduğunuz hesabı silə bilməzsiniz.";
                return RedirectToAction(nameof(Index));
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                TempData["Error"] = "İstifadəçi silinərkən xəta baş verdi.";
            }

            return RedirectToAction(nameof(Index));
        }

        // İstifadəçini bloklamaq / blokdan çıxarmaq
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsActiveStatus(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            AppUser user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            if (user.Id == _userManager.GetUserId(User))
            {
                TempData["Error"] = "Öz hesabınızı bloklaya bilməzsiniz.";
                return RedirectToAction(nameof(Index));
            }

            user.AccountStatus =
                user.AccountStatus == "Blocked" ? "Active" : "Blocked";

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                TempData["Error"] = "Hesabın statusu dəyişdirilərkən xəta baş verdi.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
