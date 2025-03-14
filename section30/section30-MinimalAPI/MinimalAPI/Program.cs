using MinimalAPI.RouteGroups;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", async (context) => await context.Response.WriteAsync("Get Hello World!"));
app.MapPost("/", async (context) => await context.Response.WriteAsync("Post Hello World!"));


//app.MapGet("products", () => JsonSerializer.Serialize(products));

var prGroup = app.MapGroup("/products");
prGroup.MapEndpoints();

app.Run();


