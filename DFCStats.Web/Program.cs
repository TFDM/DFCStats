using DFCStats.Business;
using DFCStats.Business.Interfaces;
using DFCStats.Data;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

var builder = WebApplication.CreateBuilder(args);

// Sets an empty variable for the connection string.
// This will be populated based on the environment the app is running under below
var darloStatsConnectionString = "";
var mailgunUsername = "";
var mailgunApiKey = "";
var mailgunDomain = "";

if (builder.Environment.IsDevelopment())
{
    // In development mode get the settings from the user secrets

    // Get the connection string for the database
    darloStatsConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    // Get the mailgun sandbox api details from user secrets
    // If you need to switch to the mail gun email service when in development
    // you will need these. If you are just using the DevEmail service these won't be needed
    mailgunApiKey = builder.Configuration["Mailgun:ApiKey"];
    mailgunDomain = builder.Configuration["Mailgun:Domain"];
    mailgunUsername = builder.Configuration["Mailgun:Username"];

} else
{
    // In production mode get settings from Azure secrets manager

    // Set client id, tenant id, secret and vault url from environment variables on the server
	var tenantId = Environment.GetEnvironmentVariable("AzureTenantId");
	var clientId = Environment.GetEnvironmentVariable("AzureClientId");
	var clientSecret = Environment.GetEnvironmentVariable("AzureSecret");
	var keyVaultUrl = Environment.GetEnvironmentVariable("AzureKeyVaultURL");

    // Create a ClientSecretCredential using the Azure AD app credentials
    var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecret);

	// Create the SecretClient
	var secretClient = new SecretClient(new Uri(keyVaultUrl!), clientSecretCredential);

    // Get the secrets from the key vault - this will include the database connection string 
    // and mail gun email api key and domain
    KeyVaultSecret dbConnectionString = secretClient.GetSecret("DatabaseConnectionString");
    KeyVaultSecret mailGunApiKey = secretClient.GetSecret("MailgunApiKey");
    KeyVaultSecret mailGunDomain = secretClient.GetSecret("MailgunDomain");

	// darloStatsConnectionString = dbConnectionString.Value;
    // mailgunApiKey = mailGunApiKey.Value;
    // mailgunDomain = mailGunDomain.Value;

    // Feed the mail gun api key and domain into IConfiguration so options binding works the same
    // way it would in Development via user secrets
    builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
    {
        ["Mailgun:ApiKey"] = mailgunApiKey,
        ["Mailgun:Domain"] = mailgunDomain
    });
}

// Add services to the container.
builder.Services.AddControllersWithViews(
    // This is required to stop the framework from adding the [Required] attribute to non-nullable reference types
    options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

// Configures Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    options.SlidingExpiration = true;
    options.AccessDeniedPath = "/Home/Error/";
    options.LoginPath = "/User/Login";
});

// Add validation - This scans the assembly where program.cs is defined
// it will find ever class that inherits from AbstractValidator<T>
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

// Add the dbcontext and specify the connection string
builder.Services.AddDbContext<DFCStatsDBContext>(options =>
    options.UseSqlServer(darloStatsConnectionString)
);

// Register the business services
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IClubService, ClubService>();
builder.Services.AddScoped<IEmailTemplateService, EmailTemplateService>();
builder.Services.AddScoped<IFixtureService, FixtureService>();
builder.Services.AddScoped<IManagerService, ManagerService>();
builder.Services.AddScoped<INationalityService, NationalityService>();
builder.Services.AddScoped<IParticipationService, ParticipationService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ISeasonService, SeasonService>();
builder.Services.AddScoped<ITableService, TableService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IVenueService, VenueService>();

// Allows the anti forgery token to be usable in request headers for internal api calls
builder.Services.AddAntiforgery(options => options.HeaderName = "__RequestVerificationToken");


if (builder.Environment.IsDevelopment())
{
    // Use the dev email service. Instead of sending actual emails
    // stuff will just be logged to the terminal instead
    builder.Services.AddScoped<IEmailService, DevEmailService>();
} else
{
    // Registers MailgunOptions with the DI container as a strongly-typed options class,
    // so it can be injected anywhere via IOptions<MailgunOptions>
    builder.Services
        // Binds the "Mailgun" section of configuration (appsettings.json, user secrets,
        // or Key Vault - wherever it ultimately comes from) to the properties on MailgunOptions.
        // e.g. config key "Mailgun:ApiKey" maps to MailgunOptions.ApiKey
        .AddOptions<MailgunOptions>()
        .Bind(builder.Configuration.GetSection("Mailgun"))
        .ValidateDataAnnotations()
        .ValidateOnStart();

    // Registers MailgunEmailService as the implementation of IEmailService,
    // and gives it a dedicated named HttpClient (managed by IHttpClientFactory)
    // that's injected directly into its constructor
    builder.Services.AddHttpClient<IEmailService, MailgunEmailService>();
}


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
