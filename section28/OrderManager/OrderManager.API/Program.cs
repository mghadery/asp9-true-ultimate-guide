using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OrderManager.API.Helpers;
using OrderManager.Core.Domain.RepositoryContracts;
using OrderManager.Core.ServiceContracts;
using OrderManager.Core.Services;
using OrderManager.Infrastructure.Persistent.DbContexts;
using OrderManager.Infrastructure.Persistent.Repositories;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services,logger) =>
    logger.ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services));

builder.Services.AddApiVersioning(options =>
{
    //options.ApiVersionReader = new UrlSegmentApiVersionReader();
    options.ReportApiVersions = true;
})
    .AddMvc()
    .AddApiExplorer(options =>
     {
         options.GroupNameFormat = "'ver'VVV"; // Format for version groups
         options.SubstituteApiVersionInUrl = true; // Substitute API version in URL
     });

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    {
        options.IncludeXmlComments(System.IO.Path.Combine(builder.Environment.ContentRootPath, @"ApiDoc.xml"));
    });


builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
    );

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
        builder.WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod());

    //options.AddPolicy("TestPolicy", builder =>
    //    builder.WithOrigins("http://localhost:4100"));
});
//builder.Services.AddControllers();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrdersService, OrdersService>();
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
        options =>
        {
            var descriptions = app.DescribeApiVersions();
            foreach (var description in descriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
            }
        });
}

app.UseRouting();
app.UseCors();


app.MapControllers();
app.MapGet("/", () => "Hello World!");

app.Run();
