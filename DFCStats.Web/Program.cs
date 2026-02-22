using DFCStats.Business;
using DFCStats.Business.Interfaces;
using DFCStats.Data;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(
    // This is required to stop the framework from adding the [Required] attribute to non-nullable reference types
    options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

// Add validation - This scans the assembly where program.cs is defined
// it will find ever class that inherits from AbstractValidator<T>
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

// Add the dbcontext
builder.Services.AddDbContext<DFCStatsDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Register the business services
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IClubService, ClubService>();
builder.Services.AddScoped<IFixtureService, FixtureService>();
builder.Services.AddScoped<INationalityService, NationalityService>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<ISeasonService, SeasonService>();
builder.Services.AddScoped<IVenueService, VenueService>();

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
