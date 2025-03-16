using Microsoft.EntityFrameworkCore;
using OrderManager.Core.Domain.RepositoryContracts;
using OrderManager.Core.ServiceContracts;
using OrderManager.Core.Services;
using OrderManager.Infrastructure.Persistent.DbContexts;
using OrderManager.Infrastructure.Persistent.Repositories;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
    );

builder.Services.AddControllers();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrdersService, OrdersService>();
var app = builder.Build();


app.MapControllers();
app.MapGet("/", () => "Hello World!");

app.Run();
