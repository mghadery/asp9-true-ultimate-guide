using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using People.Entities;
using People.Persistent;
using People.ServiceContracts.Interfaces;
using People.ServiceContracts.Repositories;
using People.Services;
using Rotativa.AspNetCore;
using Section15_16_17.Practice.Web.Filters.Filters;
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

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();

public partial class Program { }
