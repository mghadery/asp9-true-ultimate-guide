using Section4.Assignment6;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddTransient<LoginMiddleware2>();  //no need for LoginMiddleware!

var app = builder.Build();


//custom error handling
app.Use(async (context, next) =>
{
    if (context.Request.Path != "/")
    {
        context.Response.StatusCode = (int)StatusCodes.Status404NotFound;
        await context.Response.WriteAsync("Not Found");
    }
    else if (context.Request.Method == "POST")
    {
        await next(context);
    }
}
);

//terminating custom middleware
app.UseLoginMiddleware();
//app.UseLoginMiddleware2();


app.Run(async context =>
{
    context.Response.StatusCode = (int)StatusCodes.Status200OK;
    await context.Response.WriteAsync("No Content");
});

app.Run();
