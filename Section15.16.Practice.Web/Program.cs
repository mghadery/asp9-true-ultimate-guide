using Section15.Practice.ServiceContracts.Interfaces;
using Section15.Practice.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IPersonsService, PersonsService>();
builder.Services.AddSingleton<ICountriesService, CountriesService>();
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
