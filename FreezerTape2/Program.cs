using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FreezerTape2.Data;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("FreezerContext");

builder.Services.AddDbContext<FreezerContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
    // The following three options help with debugging, but should
    // be changed or removed for production.
    .LogTo(Console.WriteLine, LogLevel.Information)
    .EnableSensitiveDataLogging()
    .EnableDetailedErrors()
);

if (!FreezerContext.IsMigrationChecked)
{
    DatabaseMigrator.Migrate();
}


//DatabaseMigrator.Migrate();

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

// Make the application available through a subpath.
string? basePath = Environment.GetEnvironmentVariable("BASEPATH");
if (!string.IsNullOrEmpty(basePath))
{
    app.UsePathBase(basePath);
}

// https://stackoverflow.com/questions/57016996/how-to-use-userequestlocalization-in-asp-net-core-3
var supportedCultures = new string[] { "sv-SE" };
app.UseRequestLocalization(options =>
        options
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures)
        .SetDefaultCulture("sv-SE")
        .RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(context =>
        {
            return Task.FromResult(new ProviderCultureResult("sv-SE"));
        }))
    );


app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "/{controller=Packages}/{action=Index}/{id?}");

app.Run();
