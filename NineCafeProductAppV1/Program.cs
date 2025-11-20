using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NineCafeProductAppV1.Data;
using NineCafeProductAppV1.Models;
using NineCafeProductAppV1.Repositories;

namespace NineCafeProductAppV1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // register appdbcontext <--
            builder.Services.AddDbContext<AppDbContext>(
                options => options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            // register identity  <--
            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            }).AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            // register service (IRepository) <--
            builder.Services.AddScoped<IRepository<ProductPosting>, ProductRepository>();


            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // scope <--
            using(var scope = app.Services.CreateScope())
            {
                var service = scope.ServiceProvider;

                RoleSeeder.SeedRoleAsync(service).Wait();
                UserSeeder.UserSeederAsync(service).Wait();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles(); // <--
            app.UseRouting();

            app.UseAuthorization();
            app.MapRazorPages(); // <--

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Products}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
