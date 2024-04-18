using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.OpenSsl;
using WebProject.interfaces;
using WebProject.interfaces.auth;
using WebProject.interfaces.Repositories;
using WebProject.Models;
using WebProject.Provaiders;
using WebProject.Provaiders.options;
using WebProject.Repositories;
using WebProject.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddSingleton<UserService>((serivce) =>
        {
            ICustomPasswordHasher customPasswordHasher = new CustomPasswordHasher();
            IJwtProvider _jwtProvider = new JwtProvider(configuration: builder.Configuration);
            IUserRepository _userRepository = new UserRepository(new mysqlDBService(builder.Configuration.GetValue<string>("mysql:connectionString")));

            return new UserService(customPasswordHasher, _jwtProvider, _userRepository);
        });

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
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