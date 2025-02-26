using Section12.Assignment22.Interfaces;
using Section12.Assignment22.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IWeatherService, WeatherService>();
var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
