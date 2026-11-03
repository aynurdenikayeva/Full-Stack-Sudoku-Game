using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SudokuExamProject.DataAccessLayer;
using SudokuExamProject.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SudokuExamProject.Controllers
{
    public class SignUpController : Controller
    {
        private readonly AppDbContext _context;

        public SignUpController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("/signup")]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost("/signup")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(SignUp model)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", model);
            }

            string email = model.Email.Trim();

            bool emailExists = await _context.Users
                .AnyAsync(u => u.Email.ToLower() == email.ToLower());

            if (emailExists)
            {
                ModelState.AddModelError(
                    nameof(model.Email),
                    "Bu e-poçt ünvanı artıq qeydiyyatdan keçib."
                );

                return View("Register", model);
            }

            var user = new User
            {
                Name = model.Name.Trim(),
                Surname = "",
                UserName = email,
                Email = email,
                TelephonNumber = model.TelephonNumber ?? "",
                UserRole = "Player",
                SudokuLevel = "Beginner",
                AccountStatus = "Active",
                Note = ""
            };

            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, model.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Qeydiyyat uğurla tamamlandı. İndi daxil ola bilərsiniz.";
            return RedirectToAction(nameof(Register));
        }

        // ==========================================
        // SOSİAL ŞƏBƏKƏ (GOOGLE / GITHUB) METODLARI
        // ==========================================

        [HttpGet]
        public IActionResult ExternalLogin(string provider)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "SignUp");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string remoteError = null)
        {
            if (remoteError != null)
            {
                TempData["Error"] = $"Xarici provayderdən xəta gəldi: {remoteError}";
                return RedirectToAction(nameof(Register));
            }

            // Brauzerə yazılmış xarici autentifikasiya məlumatlarını (məs. Google identity) oxuyuruq
            // Google və ya GitHub-dan gələn məlumatları müvafiq sxem adları ilə yoxlayırıq
            var authenticateResult = await HttpContext.AuthenticateAsync("Google") ??
                                     await HttpContext.AuthenticateAsync("GitHub");

            if (authenticateResult == null || !authenticateResult.Succeeded)
            {
                TempData["Error"] = "Xarici provayder məlumatları oxuna bilmədi.";
                return RedirectToAction(nameof(Register));
            }

            var claimsPrincipal = authenticateResult.Principal;
            var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);
            var name = claimsPrincipal.FindFirstValue(ClaimTypes.Name) ?? "Sosial İstifadəçi";

            if (string.IsNullOrEmpty(email))
            {
                TempData["Error"] = "Sosial şəbəkədən e-poçt ünvanı alına bilmədi.";
                return RedirectToAction(nameof(Register));
            }

            // 1. Bazada bu istifadəçinin olub-olmadığını yoxlayırıq
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

            if (user == null)
            {
                user = new User
                {
                    Name = name,
                    Surname = "",
                    UserName = email,
                    Email = email,
                    TelephonNumber = "",
                    UserRole = "Player",
                    SudokuLevel = "Beginner",
                    AccountStatus = "Active",
                    PasswordHash = "",
                    Note = "Sosial şəbəkə ilə qeydiyyat"
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            // 2. CUSTOM COOKIE SIGN-IN (Sistemə Giriş Mexanizmi)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.UserRole)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            };

            // Brauzerə əsas Cookie məlumatını yazırıq və istifadəçi daxil olur
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            // DÜZƏLİŞ: Xətaya səbəb olan Google və GitHub SignOut sətirləri buradan tamamilə silindi.

            return RedirectToAction("Index", "Home");
        }

        // Sistemdən təhlükəsiz çıxış metodu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Brauzerdəki cookie-ni silir və autentifikasiyanı sonlandırır
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // Əgər istifadə olunubsa sessiyanı təmizləyir
            HttpContext.Session.Clear();
            // İstifadəçini əsas səhifəyə (Home/Index) yönləndirir
            return RedirectToAction("Index", "Home");
        }
    } 
} 



    

