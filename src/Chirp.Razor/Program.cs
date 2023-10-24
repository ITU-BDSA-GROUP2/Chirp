using Microsoft.EntityFrameworkCore;
using EFCore;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Vælg MIT license
// Opdater eller slet issue workflow 
// chrip_cli branch 

// Add services to the container.
//builder.Services.AddDbContext<ChirpDBContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Chirp")));

//builder.Services. noget med scope



// The following configures EF to create a Sqlite database file in the
// special "local" folder for your platform.
//builder.Services.AddDbContext<ChirpDBContext>(options => options.UseSqlite($"Data Source={DbPath}"));


builder.Services.AddRazorPages();
builder.Services.AddDbContext<ChirpDBContext>();
builder.Services.AddScoped<ICheepRepository, CheepRepository>();
// Tror den skal se således ud
//builder.Services.AddSingleton<ICheepRepository, CheepRepository>();


var app = builder.Build();

using (var scope = app.Services.CreateScope()){
    
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

app.MapRazorPages();

app.Run();
