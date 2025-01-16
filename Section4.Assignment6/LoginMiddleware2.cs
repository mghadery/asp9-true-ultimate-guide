using System.Text.RegularExpressions;

namespace Section4.Assignment6;

public class LoginMiddleware2:IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var bodyStr = context.Request.Body;
        string body;
        using (StreamReader strr = new StreamReader(bodyStr))
        {
            body = await strr.ReadToEndAsync();
        }

        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(body);
        string? email = query.ContainsKey("email") ? query?["email"][0] : null;
        string? password = query.ContainsKey("password") ? query?["password"][0] : null;
        List<string> errors = new();
        if (query == null || email is null ||
            !new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$").IsMatch(email))
            errors.Add("Invalid input for email");
        if (query == null || password is null)
            errors.Add("Invalid input for password");

        if (errors.Count > 0)
        {
            context.Response.StatusCode = (int)StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync(string.Join(Environment.NewLine, errors));
            return;
        }


        if (email == "admin@example.com" && password == "admin1234")
        {
            context.Response.StatusCode = (int)StatusCodes.Status200OK;
            await context.Response.WriteAsync("Successful Login");
        }
        else
        {
            context.Response.StatusCode = (int)StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Invalid Login");
        }
    }
}

public static class LoginMiddlewareExtension2
{
    public static IApplicationBuilder UseLoginMiddleware2(this IApplicationBuilder app)
    {
        return app.UseMiddleware<LoginMiddleware2>();
    }
}

