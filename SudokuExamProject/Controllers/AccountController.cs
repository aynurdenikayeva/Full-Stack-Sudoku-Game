using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SudokuExamProject.Models;
using SudokuExamProject.ViewModels;
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

        [HttpGet("/signup")]
        [HttpGet("Account/Register")]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost("/signup")]
        [HttpPost("Account/Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            await CreateRoleDB();

            if (!ModelState.IsValid)
            {
                return View("Register", model);
            }

            string email = model.Email.Trim();

            AppUser appUser = new AppUser
            {
                Name = model.Name.Trim(),
                UserName = email, // Boşluq xətası olmaması üçün Email daha uyğundur
                Email = email
            };

            // .Result yerinə await istifadə edildi
            IdentityResult result = await _userManager.CreateAsync(appUser, model.Password);

            if (result.Succeeded)
            {
                // İlk istifadəçiyə Admin, digərlərinə Member rolu verilir
                var userCount = _userManager.Users.Count();
                if (userCount == 1)
                {
                    await _userManager.AddToRoleAsync(appUser, Helpers.Helpers.CreateRole.Admin.ToString());
                }
                else
                {
                    await _userManager.AddToRoleAsync(appUser, Helpers.Helpers.CreateRole.Member.ToString());
                }

                await _signInManager.SignInAsync(appUser, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            // Xəta varsa ekrana çıxarır və metod burada bitir
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("Register", model);
        }

        private async Task CreateRoleDB()
        {
            string adminRole = Helpers.Helpers.CreateRole.Admin.ToString();
            string memberRole = Helpers.Helpers.CreateRole.Member.ToString();

            if (!await _roleManager.RoleExistsAsync(adminRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(adminRole));
            }
            if (!await _roleManager.RoleExistsAsync(memberRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(memberRole));
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginModel);
            }

            AppUser appUser = await _userManager.FindByEmailAsync(loginModel.Email.Trim());
            if (appUser == null)
            {
                ModelState.AddModelError("", "Email və ya şifrə səhvdir");
                return View(loginModel);
            }

            var result = await _signInManager.PasswordSignInAsync(appUser, loginModel.Password, true, true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Hesabınız müvəqqəti bloklanıb.");
                }
                else
                {
                    ModelState.AddModelError("", "Email və ya şifrə səhvdir");
                }
                return View(loginModel);
            }

            return RedirectToAction("Index", "Home");
        }

        // SOSİAL ŞƏBƏKƏ (GOOGLE / GITHUB)
        [HttpGet]
        public IActionResult ExternalLogin(string provider)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
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

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                TempData["Error"] = "Xarici provayder məlumatları oxuna bilmədi.";
                return RedirectToAction(nameof(Register));
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider,
                info.ProviderKey,
                isPersistent: true,
                bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var name = info.Principal.FindFirstValue(ClaimTypes.Name) ?? "Sosial İstifadəçi";

            if (string.IsNullOrEmpty(email))
            {
                TempData["Error"] = "Sosial şəbəkədən e-poçt ünvanı alına bilmədi.";
                return RedirectToAction(nameof(Register));
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                await CreateRoleDB();

                user = new AppUser
                {
                    UserName = email,
                    Email = email,
                    Name = name,
                    EmailConfirmed = true
                };

                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    TempData["Error"] = "İstifadəçi hesabı yaradılarkən xəta baş verdi.";
                    return RedirectToAction(nameof(Register));
                }

                // Sosial girişlə gələn ilk/yeni istifadəçiyə də rol təyin edilir
                var userCount = _userManager.Users.Count();
                string role = userCount == 1
                    ? Helpers.Helpers.CreateRole.Admin.ToString()
                    : Helpers.Helpers.CreateRole.Member.ToString();

                await _userManager.AddToRoleAsync(user, role);
            }

            await _userManager.AddLoginAsync(user, info);
            await _signInManager.SignInAsync(user, isPersistent: true);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
