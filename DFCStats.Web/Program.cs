using DFCStats.Business;
using DFCStats.Business.Interfaces;
using DFCStats.Data;
using DFCStats.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add the dbcontext
builder.Services.AddDbContext<DFCStatsDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Register the unit of work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register the business services
builder.Services.AddScoped<IClubService, ClubService>();

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
