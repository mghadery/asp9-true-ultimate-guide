using Section14.Practice.Interfaces;
using Section14.Practice.Models;
using Section14.Practice.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.Configure<ApiClientInfo>(builder.Configuration.GetSection("ClientInfo"));
//builder.Host.ConfigureAppConfiguration(
//    b => b.AddJsonFile("myappsettings.json", optional: true));
builder.Configuration.AddJsonFile("myappsettings.json", optional: true, reloadOnChange: true);

builder.Services.AddHttpClient();

var app = builder.Build();

//does not work!
//ApiClientInfo clientInfo = app.Configuration.GetValue<ApiClientInfo>("ClientInfo");

//if (app.Environment.EnvironmentName == "Development")
//if (app.Environment.IsDevelopment())
if (app.Environment.IsEnvironment("Development"))
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();

//app.MapGet("/", async context => await context.Response.WriteAsync(app.Configuration["Key1"] ?? "No Key1 config"));

app.MapGet("/getx", async context => await context.Response.WriteAsync
    (app.Configuration.GetValue<int>("x", 10).ToString()));
//app.UseEndpoints(endpoints => { });

app.MapControllers();

app.Run();
