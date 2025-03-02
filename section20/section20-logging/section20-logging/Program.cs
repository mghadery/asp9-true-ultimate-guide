using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddDebug();
builder.Logging.AddConsole();

builder.Services.AddControllersWithViews();

builder.Host.UseSerilog((context, services, loggerConfiguration)
    => loggerConfiguration.ReadFrom.Services(services)
    //.Enrich.FromLogContext()
    .ReadFrom.Configuration(context.Configuration));

//builder.Services.AddHttpLogging(options =>
//{
//    options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.Request |
//         Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.Response;
//});

var app = builder.Build();

app.Logger.LogDebug("debug message");
app.Logger.LogError("error message");
app.Logger.LogCritical("critical message");

app.UseSerilogRequestLogging();

//app.UseHttpLogging();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
