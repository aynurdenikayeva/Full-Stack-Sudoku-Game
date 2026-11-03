using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SudokuExamProject.DataAccessLayer;
using SudokuExamProject.Services;
using Microsoft.AspNetCore.Authentication.Cookies; // YENİ: Identity-siz Cookie idarəetməsi üçün vacibdir

namespace SudokuExamProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // MVC
            services.AddControllersWithViews();

            // Session üçün keş xidməti
            services.AddDistributedMemoryCache();

            // İstifadəçi sessiyası
            services.AddSession(options =>
            {
                options.IdleTimeout = System.TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Sudoku xidməti
            services.AddSingleton<SudokuService>();

            // PostgreSQL bağlantısı
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString("PostgresConnection")
                )
            );

            // E-poçt xidməti
            services.AddScoped<EmailService>();

            // NOTE: services.AddIdentity() bütövlüklə buradan silindi!

            // SOSİAL ŞƏBƏKƏLƏR ÜÇÜN GİZLİ VƏ TƏHLÜKƏSİZ OXUMA STRUKTURU (IDENTITY-SİZ DOĞRU VARIANT)
            services.AddAuthentication(options =>
            {
                // Identity silindiyi üçün standart giriş növünü Cookie Authentication təyin edirik
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/SignIn"; // İstifadəçi icazəsiz səhifəyə girəndə yönlənəcək link
            })
            .AddGoogle(options =>
            {
                options.ClientId = Configuration["Authentication:Google:ClientId"];
                options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            })
            .AddGitHub(options =>
            {
                options.ClientId = Configuration["Authentication:GitHub:ClientId"];
                options.ClientSecret = Configuration["Authentication:GitHub:ClientSecret"];
            });
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Endpoint-lərdən əvvəl Session aktivləşdirilir
            app.UseSession();

            // Giriş məlumatlarının daxili API tərəfindən oxunması üçün mütləq bura yazılmalıdır
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // Admin paneli
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
                );

                // Əsas MVC marşrutu
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
