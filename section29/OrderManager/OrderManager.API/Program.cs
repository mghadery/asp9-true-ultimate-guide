using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OrderManager.API.Helpers;
using OrderManager.Core.Domain.RepositoryContracts;
using OrderManager.Core.ServiceContracts;
using OrderManager.Core.Services;
using OrderManager.Infrastructure.Persistent.DbContexts;
using OrderManager.Infrastructure.Persistent.IdentityEntities;
using OrderManager.Infrastructure.Persistent.Repositories;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IdentityModel.Tokens.Jwt;

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
builder.Services.AddControllers();

builder.Services.AddIdentity<User, Role>(Options =>
{
    Options.Password.RequireNonAlphanumeric = false;
    Options.Password.RequireLowercase = false;
    Options.Password.RequireUppercase = false;
    Options.Password.RequiredUniqueChars = 0;
    Options.Password.RequiredLength = 4;
    Options.Password.RequireDigit = false;
}
)
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddUserStore<UserStore<User, Role, AppDbContext, Guid>>()
    .AddRoleStore<RoleStore<Role, AppDbContext, Guid>>();

builder.Services.AddAuthorization(
    options=>
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()
    );

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrdersService, OrdersService>();

builder.Services.AddScoped<IIdentityRepository, IdentityRepository>();
builder.Services.AddScoped<IIdentityService, IdentityService>();

builder.Services.AddTransient<IJwtService, JwtService>();

//builder.Services.Configure<ApiBehaviorOptions>(options =>
//{
//    options.SuppressModelStateInvalidFilter = false;  // Ensure automatic 400 is enabled
//});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer"),
            ValidateAudience = true,
            ValidAudience = builder.Configuration.GetValue<string>("Jwt:Audience"),
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Jwt:Key")))
        };
    });
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "Hello World!");

app.Run();
