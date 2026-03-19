using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Data.interfaces;
using Shop.Data.Models;
using Shop.Data.Repository;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;

        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();

        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDBContent>()
            .AddDefaultTokenProviders()
            .AddDefaultUI();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Identity/Account/Login";
            options.AccessDeniedPath = "/Identity/Account/AccessDenied";
        });

        builder.Services.AddDbContext<AppDBContent>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddTransient<IAllCars, CarRepository>();
        builder.Services.AddTransient<ICarsCategory, CategoryRepository>();

        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddScoped(ShopCart.GetCart);
        builder.Services.AddMemoryCache();
        builder.Services.AddSession();

        builder.Services.Configure<FormOptions>(options =>
        {
            options.ValueLengthLimit = int.MaxValue;
            options.MultipartBodyLengthLimit = int.MaxValue;
            options.MemoryBufferThreshold = int.MaxValue;
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseSession();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string adminEmail = "admin@example.com";
            string adminPassword = "Admin123!";
            string adminRole = "Admin";

            if (!await roleManager.RoleExistsAsync(adminRole))
                await roleManager.CreateAsync(new IdentityRole(adminRole));

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new IdentityUser { UserName = adminEmail, Email = adminEmail };
                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, adminRole);
            }

            var content = scope.ServiceProvider.GetRequiredService<AppDBContent>();
            content.Database.EnsureCreated();
            DBObjects.Initial(content);
        }

        app.MapWhen(context => context.Request.Path == "/",
            appBranch =>
            {
                appBranch.Run(async context =>
                {
                    context.Response.Redirect("/Home/Index", permanent: false);
                });
            });

        app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
        );

        app.MapControllerRoute(
            name: "categoryFilter",
            pattern: "Car/{action}/{category?}",
            defaults: new { Controller = "Car", action = "List" });

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.MapRazorPages();

        app.Run();
    }
}