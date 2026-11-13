using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SudokuExamProject.Models;
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
            if (!ModelState.IsValid)
            {
                return View("Register", model);
            }
            if (!await _roleManager.RoleExistsAsync(Helpers.Helpers.CreateRole.Admin.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole(Helpers.Helpers.CreateRole.Admin.ToString()));
            }
            if (!await _roleManager.RoleExistsAsync(Helpers.Helpers.CreateRole.Member.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole(Helpers.Helpers.CreateRole.Member.ToString()));
            }

            string email = model.Email.Trim();

            AppUser appUser = new AppUser
            {
                Name = model.Name.Trim(),
                UserName = model.Name.Trim(),
                Email = email
            };

            // Identity şifrəni özü şifrələyib bazaya yazır və e-poçtun təkrar olub-olmadığını yoxlayır
            IdentityResult result = _userManager.CreateAsync(appUser, model.Password).Result;

            if (result.Succeeded)
            {
                // İstəsəniz istifadəçiyə standart rol təyin edə bilərsiniz (əvvəlcədən bazada "Player" rolu varsa)
                // await _userManager.AddToRoleAsync(user, "Player");

                TempData["Success"] = "Qeydiyyat uğurla tamamlandı. İndi daxil ola bilərsiniz.";
                return View(model);
            }

            // Əgər parol zəifdirsə və ya e-poçt artıq varsa, Identity xətalarını ekrana çıxarırıq
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            await _signInManager.SignInAsync(appUser, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        // ==========================================
        // SOSİAL ŞƏBƏKƏ (GOOGLE / GITHUB) METODLARI
        // ==========================================


        [HttpGet]
        public IActionResult ExternalLogin(string provider)
        {
            // Controller adı "Account" olaraq düzəldildi
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

            // Google və ya GitHub-dan gələn məlumatları Identity vasitəsilə alırıq
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                TempData["Error"] = "Xarici provayder məlumatları oxuna bilmədi.";
                return RedirectToAction(nameof(Register));
            }

            // İstifadəçi daha əvvəl bu sosial hesabla daxil olubsa, birbaşa giriş edir
            var signInResult = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider, info.ProviderKey, isPersistent: true, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            // İlk dəfə daxil olursa, e-poçtunu alıb bazada yeni AppUser yaradırıq
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
            }

            // Sosial hesabı (Google/GitHub) bazadakı istifadəçiyə bağlayırıq (AspNetUserLogins cədvəli)
            await _userManager.AddLoginAsync(user, info);
            await _signInManager.SignInAsync(user, isPersistent: true);

            return RedirectToAction("Index", "Home");
        }

        // Sistemdən təhlükəsiz çıxış metodu
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
