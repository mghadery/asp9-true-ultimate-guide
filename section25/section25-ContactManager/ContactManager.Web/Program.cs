using ContactManager.Core.Domain.RepositorieContracts;
using ContactManager.Core.ServiceContracts;
using ContactManager.Core.Services;
using ContactManager.Infrastructure.Persistent.DbContexts;
using ContactManager.Infrastructure.Persistent.Entities;
using ContactManager.Infrastructure.Persistent.Repositories;
using ContactManager.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Rotativa.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, services, logger) =>
logger.ReadFrom.Services(services)
.ReadFrom.Configuration(context.Configuration));
builder.Services.AddControllersWithViews();
//options =>
//{
//    var logger = builder.Services.BuildServiceProvider().GetService<ILogger<PersonsListActionFilter>>();
//    options.Filters.Add(new PersonsListActionFilter(logger, 100, 200));
//}
//);
builder.Services.AddTransient<PersonsListActionFilter>();
builder.Services.AddScoped<IPersonsService, PersonsService>();
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IIdentityRepository, IdentityRepository>();
builder.Services.AddScoped<IIdentityService, IdentityService>();

builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddUserStore<UserStore<User, Role, AppDbContext, Guid>>()
    .AddRoleStore<RoleStore<Role, AppDbContext, Guid>>();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    options.AddPolicy("p1", options => options.RequireRole("RoleName"));
    options.AddPolicy("NotLoggedIn", opt => opt.RequireAssertion(context =>
    !context.User.Identity.IsAuthenticated));
}
);

builder.Services.ConfigureApplicationCookie(options =>
    options.LoginPath = "/Account/Login"
);

if (!builder.Environment.IsEnvironment("Test"))
{
    builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
}
var app = builder.Build();

if (!app.Environment.IsEnvironment("Test"))
{
    var rootPath = app.Environment.WebRootPath;
    var wkhtmltopdfRelativePath = "Rotativa";
    RotativaConfiguration.Setup(rootPath, wkhtmltopdfRelativePath);
}

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

var sss = app.Configuration["sss"];

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
//app.MapControllers();
app.MapControllerRoute("default", "{controller}/{action}/{id?}");

app.Run();

public partial class Program { }
