using Microsoft.EntityFrameworkCore;
using EFCore;
using Infrastructure;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
public class Program
{

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Vælg MIT license
        // Opdater eller slet issue workflow 
        // chrip_cli branch 

        // Add services to the container.
        //builder.Services.AddDbContext<ChirpDBContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Chirp")));



        builder.Services.AddRazorPages();

        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        var DbPath = System.IO.Path.Join(path, "chirp.db");

        builder.Services.AddDbContext<ChirpDBContext>(options => options.UseSqlite($"Data source={DbPath}"));

        builder.Services.AddScoped<ICheepRepository, CheepRepository>();
        builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();


        builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
         options.Password.RequiredLength = 8)
         .AddEntityFrameworkStores<ChirpDBContext>()
         .AddDefaultUI()
         .AddDefaultTokenProviders();
        builder.Services.AddAuthentication(options =>
            {
                // options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                // options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                // options.DefaultChallengeScheme = "GitHub";
            })
            //.AddCookie();
            .AddGitHub(o =>
            {
                o.ClientId = builder.Configuration["authentication:github:clientId"];
                o.ClientSecret = builder.Configuration["authentication:github:clientSecret"];
            });


        // Tror den skal se således ud
        //builder.Services.AddSingleton<ICheepRepository, CheepRepository>();


        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {

            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<ChirpDBContext>();
            DbInitializer.SeedDatabase(context);
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapRazorPages();
        app.UseCookiePolicy(new CookiePolicyOptions()
        {
            MinimumSameSitePolicy = SameSiteMode.Lax
        });
        app.Run();
    }
}