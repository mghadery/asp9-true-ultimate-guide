using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Stocks.Models;
using Stocks.Persistent;
using Stocks.ServiceContracts.Interfaces;
using Stocks.Services;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IFinnhubService, FinnhubService>();
builder.Services.AddScoped<IStocksService, StocksService>();
builder.Services.AddHttpClient();
builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection("TradingOptions"));
builder.Services.AddDbContext<StocksDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped(typeof(IStocksRepo<>), typeof(StocksRepo<>));
var app = builder.Build();

RotativaConfiguration.Setup(app.Environment.ContentRootPath, "wwwroot");

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
