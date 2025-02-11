var builder = WebApplication.CreateBuilder(new WebApplicationOptions() { WebRootPath = "webroot" });
builder.Services.AddControllersWithViews();
var app = builder.Build();

app.MapControllers();

app.Run();
