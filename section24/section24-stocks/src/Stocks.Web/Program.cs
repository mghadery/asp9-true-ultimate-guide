using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Serilog;
using Stocks.Core.Domain.RepositoryContracts;
using Stocks.Core.DTOs;
using Stocks.Core.FinnhubServiceContracts;
using Stocks.Core.Services;
using Stocks.Infrastructure.DbContexts;
using Stocks.Infrastructure.FinnhubServices;
using Stocks.Infrastructure.Repositories;
using Stocks.ServiceContracts.Interfaces;
using Stocks.Web.Middleware;


var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, services, configureLogger) =>
  configureLogger.ReadFrom.Services(services)
  .ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IFinnhubService, FinnhubService>();
builder.Services.AddScoped<IStocksGetService, StocksGetService>();
builder.Services.AddScoped<IStocksCreateService, StocksCreateService>();
builder.Services.AddHttpClient();
builder.Services.Configure<TradingOptions>(builder.Configuration.GetSection("TradingOptions"));
builder.Services.AddDbContext<StocksDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped(typeof(IStocksRepo<>), typeof(StocksRepo<>));
var app = builder.Build();

RotativaConfiguration.Setup(app.Environment.ContentRootPath, "wwwroot");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseCustomExceptionHandler();
}

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
