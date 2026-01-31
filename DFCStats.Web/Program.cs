using DFCStats.Business;
using DFCStats.Business.Interfaces;
using DFCStats.Data;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add validation - This scans the assembly where program.cs is defined
// it will find ever class that inherits from AbstractValidator<T>
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddFluentValidationAutoValidation();

// Add the dbcontext
builder.Services.AddDbContext<DFCStatsDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Register the business services
builder.Services.AddScoped<IClubService, ClubService>();
builder.Services.AddScoped<ISeasonService, SeasonService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Seed the database with sample data if running in development mode
if (app.Environment.IsDevelopment())
{
    await DFCStats.Data.DbSeeder.SeedAsync(app.Services);
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
