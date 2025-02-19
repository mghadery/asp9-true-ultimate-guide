using Section13.Practice.Interfaces;
using Section13.Practice.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IWeatherService, WeatherService>();
var app = builder.Build();

//if (app.Environment.EnvironmentName == "Development")
//if (app.Environment.IsDevelopment())
if (app.Environment.IsEnvironment("Development"))
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.MapControllers();

app.Run();
