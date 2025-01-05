using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityBasics
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host
                .CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Introduction:
            // ASP.NET Core Identity is a membership system for managing users in web applications.
            // It handles user registration, login, roles, and other user-related functionality.
            //
            // Use Cases:
            // - **User Registration**: Allow users to create accounts.
            // - **Authentication**: Verify users during login.
            // - **Role-Based Authorization**: Grant or restrict access based on roles.
            // - **Password Recovery**: Help users recover forgotten passwords.
            //
            // This chapter covers:
            // - Setting up ASP.NET Core Identity with a SQLite database.
            // - Basic user management (register, login, roles).

            // Configure Entity Framework with SQLite (so that we have a database to store user data - aka User Repository).
            services
                .AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("InMemoryDb"));

            // Configure ASP.NET Core Identity.
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
                {
                    // Customize password requirements
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 3;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequiredUniqueChars = 0;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add controllers with views for identity pages.
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // Explanation:
            // Enable authentication and authorization middleware.
            // These are required for managing user access and roles.
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // Map default controller routes.
                endpoints.MapDefaultControllerRoute();
            });

            // Seed the database with default roles and an admin user.
            SeedDatabase(app);
        }

        private void SeedDatabase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Ensure roles exist.
                if (!roleManager.RoleExistsAsync("Admin").Result)
                {
                    roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
                }

                if (!roleManager.RoleExistsAsync("User").Result)
                {
                    roleManager.CreateAsync(new IdentityRole("User")).Wait();
                }

                // Create a default admin user if it doesn't exist.
                var adminUser = userManager.FindByEmailAsync("admin@example.com").Result;
                if (adminUser == null)
                {
                    adminUser = new IdentityUser
                    {
                        UserName = "admin",
                        Email = "admin@example.com",
                        EmailConfirmed = true
                    };
                    userManager.CreateAsync(adminUser, "admin").Wait();
                    userManager.AddToRoleAsync(adminUser, "Admin").Wait();
                }
            }
        }
    }

    // ApplicationDbContext: Represents the database context for Identity.
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }

    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewData["Message"] = $"Hello, {User.Identity.Name}!";
            }
            else
            {
                ViewData["Message"] = "Hello, anonymous user!";
            }
            return View();
        }
    }

    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            var user = new IdentityUser { UserName = username };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
