using Domain.Data;
using Domain.Data.Models;
using Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace UrlShortener
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var localDbConnectionString = builder.Configuration.GetConnectionString("UrlShortenerContext");
            builder.Services.AddDbContext<UrlShortenerContext>(options => options.UseSqlServer(localDbConnectionString));

            builder.Services.AddScoped<IRepository<ShortUri>, ShortUriRepository>();
            builder.Services.AddScoped<Random, Random>();
            builder.Services.AddScoped<IShortUriCreator, ShortUriCreator>();

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

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<UrlShortenerContext>();
                context.Database.EnsureCreated();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
