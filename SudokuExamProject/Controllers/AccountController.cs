using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SudokuExamProject.Models;
using SudokuExamProject.ViewModels;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SudokuExamProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // ROLLARIN YARADILMASI
        private async Task<bool> CreateRoleDB()
        {
            string[] roles = { "Admin", "Player" };

            foreach (string role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    var result = await _roleManager.CreateAsync(
                        new IdentityRole(role));

                    if (!result.Succeeded)
                        return false;
                }
            }

            return true;
        }

        // İLK İSTİFADƏÇİ ADMIN, SONRAKILAR PLAYER
        private async Task<string> GetNewUserRole()
        {
            bool hasUsers = await _userManager.Users.AnyAsync();

            return hasUsers ? "Player" : "Admin";
        }

        // İSTİFADƏÇİNİN BLOKLANIB-BLOKLANMADIĞINI YOXLAYIR
        private bool IsUserBlocked(AppUser user)
        {
            return string.Equals(
                user.AccountStatus,
                "Blocked",
                StringComparison.OrdinalIgnoreCase);
        }

        // REGISTER GET
        [HttpGet("/signup")]
        [HttpGet("Account/Register")]
        public IActionResult Register()
        {
            return View("Register");
        }

        // REGISTER POST
        [HttpPost("/signup")]
        [HttpPost("Account/Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Register", model);

            bool rolesCreated = await CreateRoleDB();

            if (!rolesCreated)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Rollar yaradılarkən xəta baş verdi.");

                return View("Register", model);
            }

            string email = model.Email.Trim();

            // Yeni istifadəçinin rolunu müəyyən edirik.
            string role = await GetNewUserRole();

            AppUser appUser = new AppUser
            {
                Name = model.Name.Trim(),
                UserName = email,
                Email = email,
                AccountStatus = "Active",
                IsOnline = false
            };

            var result = await _userManager.CreateAsync(
                appUser,
                model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);

                return View("Register", model);
            }

            var roleResult = await _userManager.AddToRoleAsync(
                appUser,
                role);

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(appUser);

                foreach (var error in roleResult.Errors)
                    ModelState.AddModelError(
                        string.Empty,
                        error.Description);

                return View("Register", model);
            }

            // Qeydiyyatdan sonra istifadəçini sistemə daxil edirik.
            appUser.IsOnline = true;
            await _userManager.UpdateAsync(appUser);

            await _signInManager.SignInAsync(
                appUser,
                isPersistent: false);

            return RedirectToAction("Index", "Home");
        }

        // LOGIN GET
        [HttpGet("/SignIn")]
        public IActionResult Login()
        {
            return View();
        }

        // LOGIN POST
        [HttpPost("/SignIn")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (!ModelState.IsValid)
                return View("Login", loginModel);

            string email = loginModel.Email.Trim();

            AppUser appUser = await _userManager.FindByEmailAsync(email);

            if (appUser == null)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Email və ya şifrə səhvdir.");

                return View("Login", loginModel);
            }

            // Bloklanmış istifadəçi daxil ola bilməz.
            if (IsUserBlocked(appUser))
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Hesabınız bloklanıb. Daxil olmaq mümkün deyil.");

                return View("Login", loginModel);
            }

            var result = await _signInManager.PasswordSignInAsync(
                appUser,
                loginModel.Password,
                isPersistent: true,
                lockoutOnFailure: true);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Hesabınız müvəqqəti kilidlənib.");
                }
                else
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Email və ya şifrə səhvdir.");
                }

                return View("Login", loginModel);
            }

            // Uğurlu giriş zamanı aktiv edirik.
            appUser.IsOnline = true;
            await _userManager.UpdateAsync(appUser);

            return RedirectToAction("Index", "Home");
        }

        // GOOGLE / GITHUB LOGIN
        [HttpGet]
        public IActionResult ExternalLogin(string provider)
        {
            if (provider != "Google" && provider != "GitHub")
                return BadRequest();

            var redirectUrl = Url.Action(
                nameof(ExternalLoginCallback),
                "Account");

            var properties =
                _signInManager.ConfigureExternalAuthenticationProperties(
                    provider,
                    redirectUrl);

            return Challenge(properties, provider);
        }

        // EXTERNAL LOGIN CALLBACK
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(
            string? remoteError = null)
        {
            if (remoteError != null)
            {
                TempData["Error"] =
                    "Xarici provayderdən giriş zamanı xəta baş verdi.";

                return RedirectToAction(nameof(Register));
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                TempData["Error"] =
                    "Xarici provayder məlumatları oxuna bilmədi.";

                return RedirectToAction(nameof(Register));
            }

            // Əvvəldən bu sosial hesaba bağlı istifadəçi.
            var user = await _userManager.FindByLoginAsync(
                info.LoginProvider,
                info.ProviderKey);

            if (user != null)
            {
                if (IsUserBlocked(user))
                {
                    TempData["Error"] =
                        "Hesabınız bloklanıb. Daxil olmaq mümkün deyil.";

                    return RedirectToAction(nameof(Login));
                }

                var signInResult =
                    await _signInManager.ExternalLoginSignInAsync(
                        info.LoginProvider,
                        info.ProviderKey,
                        isPersistent: true,
                        bypassTwoFactor: true);

                if (signInResult.Succeeded)
                {
                    user.IsOnline = true;
                    await _userManager.UpdateAsync(user);

                    return RedirectToAction("Index", "Home");
                }

                TempData["Error"] =
                    "Hesaba giriş mümkün olmadı.";

                return RedirectToAction(nameof(Login));
            }

            // Sosial şəbəkədən istifadəçi məlumatları.
            string? email = info.Principal.FindFirstValue(
                ClaimTypes.Email);

            string name = info.Principal.FindFirstValue(
                ClaimTypes.Name) ?? "Sosial İstifadəçi";

            if (string.IsNullOrWhiteSpace(email))
            {
                TempData["Error"] =
                    "Sosial şəbəkədən e-poçt ünvanı alına bilmədi.";

                return RedirectToAction(nameof(Register));
            }

            email = email.Trim();

            // Email ilə mövcud istifadəçini axtarırıq.
            user = await _userManager.FindByEmailAsync(email);

            bool newUserCreated = false;

            if (user == null)
            {
                bool rolesCreated = await CreateRoleDB();

                if (!rolesCreated)
                {
                    TempData["Error"] =
                        "Rollar yaradılarkən xəta baş verdi.";

                    return RedirectToAction(nameof(Register));
                }

                string role = await GetNewUserRole();

                user = new AppUser
                {
                    UserName = email,
                    Email = email,
                    Name = name,
                    EmailConfirmed = true,
                    AccountStatus = "Active",
                    IsOnline = false
                };

                var createResult = await _userManager.CreateAsync(user);

                if (!createResult.Succeeded)
                {
                    TempData["Error"] =
                        "İstifadəçi hesabı yaradılarkən xəta baş verdi.";

                    return RedirectToAction(nameof(Register));
                }

                newUserCreated = true;

                var roleResult = await _userManager.AddToRoleAsync(
                    user,
                    role);

                if (!roleResult.Succeeded)
                {
                    await _userManager.DeleteAsync(user);

                    TempData["Error"] =
                        "İstifadəçiyə rol təyin edilərkən xəta baş verdi.";

                    return RedirectToAction(nameof(Register));
                }
            }
            else
            {
                if (IsUserBlocked(user))
                {
                    TempData["Error"] =
                        "Hesabınız bloklanıb. Daxil olmaq mümkün deyil.";

                    return RedirectToAction(nameof(Login));
                }
            }

            // Sosial hesabı istifadəçiyə bağlayırıq.
            var addLoginResult = await _userManager.AddLoginAsync(
                user,
                info);

            if (!addLoginResult.Succeeded)
            {
                if (newUserCreated)
                    await _userManager.DeleteAsync(user);

                TempData["Error"] =
                    "Sosial hesab əlaqələndirilərkən xəta baş verdi.";

                return RedirectToAction(nameof(Register));
            }

            // Sosial hesabla giriş zamanı aktiv edirik.
            user.IsOnline = true;
            await _userManager.UpdateAsync(user);

            await _signInManager.SignInAsync(
                user,
                isPersistent: true);

            return RedirectToAction("Index", "Home");
        }

        // LOGOUT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                user.IsOnline = false;
                await _userManager.UpdateAsync(user);
            }

            await _signInManager.SignOutAsync();

            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
