using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ResturantManagerMVC.Data;

namespace ResturantManagerMVC
{
    public class Program
    {
        public static async Task Main(string[] args) // Change void to Task
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add Identity after DbContext
            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
                options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddRazorPages();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            await CreateRolesAndAdminUser(app); // Ensure this is awaited
            app.Run(); // Use RunAsync instead of Run
        }

        private static async Task CreateRolesAndAdminUser(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                var logger = services.GetRequiredService<ILogger<Program>>();

                var roles = new[] { "User", "Admin" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        var result = await roleManager.CreateAsync(new IdentityRole(role));
                        if (!result.Succeeded)
                        {
                            logger.LogError($"Failed to create role {role}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                        }
                    }
                }

                string adminEmail = "admin@example.com";
                string adminPassword = "Admin#123";

                if (await userManager.FindByEmailAsync(adminEmail) == null)
                {
                    var admin = new IdentityUser { UserName = adminEmail, Email = adminEmail };
                    var result = await userManager.CreateAsync(admin, adminPassword);
                    if (result.Succeeded)
                    {
                        var roleResult = await userManager.AddToRoleAsync(admin, "Admin");
                        if (!roleResult.Succeeded)
                        {
                            logger.LogError($"Failed to assign Admin role to user {adminEmail}: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                        }
                    }
                    else
                    {
                        logger.LogError($"Failed to create admin user {adminEmail}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
            }
        }
    }

}
