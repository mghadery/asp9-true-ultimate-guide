using Section17.Assignment31.Models;
using Section16.Assignment29.ServiceContracts.Interfaces;
using Section16.Assignment29.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IFinnhubService, FinnhubService>();
builder.Services.AddSingleton<IStocksService, StocksService>();
builder.Services.AddHttpClient();
builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection("TradingOptions"));
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
