var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRouting(options =>
    options.ConstraintMap.Add("country", typeof(CountryConstraint)));
var app = builder.Build();

Dictionary<int, string> countries = new()
{
    [1] = "United States",
    [2] = "Canada",
    [3] = "United Kingdom",
    [4] = "India",
    [5] = "Japan"
};

app.MapGet("/countries/{countryId:country?}", async context =>
{
    string? countryIdStr = context.Request.RouteValues["countryId"]?.ToString();
    if (countryIdStr == null)
    {
            await context.Response.WriteAsync(string.Join(Environment.NewLine, 
                countries.Select( x=> x.Key + ", " + x.Value)
                ));
    }
    else
    {
        int countryId = int.Parse(countryIdStr);
        await context.Response.WriteAsync(countries[countryId]);
    }
});

app.MapGet("/countries/{countryId:int:min(101)}", async (context) =>
{
    context.Response.StatusCode = (int)StatusCodes.Status400BadRequest;
    await context.Response.WriteAsync("The id must be from 1 to 100");
});

app.MapGet("/countries/{countryId:int:max(0)}", async (context) =>
{
    context.Response.StatusCode = (int)StatusCodes.Status400BadRequest;
    await context.Response.WriteAsync("The id must be from 1 to 100");
});

app.MapGet("/countriesm2/{countryId:int?}", async context =>
{
    string? countryIdStr = context.Request.RouteValues["countryId"]?.ToString();
    if (countryIdStr == null)
    {
        await context.Response.WriteAsync(string.Join(Environment.NewLine,
            countries.Select(x => x.Key + ", " + x.Value)
            ));
        return;
    }

    int countryId = int.Parse(countryIdStr);
    

    if (countryId < 1 || countryId > 100)
    {
        context.Response.StatusCode = (int)StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync("The id must be from 1 to 100");
    }
    else if (!countries.ContainsKey(countryId))
    {
        context.Response.StatusCode = (int)StatusCodes.Status404NotFound;
        await context.Response.WriteAsync("Id not found");
    }
    else
    {
        context.Response.StatusCode = (int)StatusCodes.Status200OK;
        await context.Response.WriteAsync(countries[countryId]);
    }
});

app.Run();

public class CountryConstraint : IRouteConstraint
{
    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (!values.ContainsKey(routeKey)) return false; //if the parameter empty value
                                                         //is permitted in the parameter constraint,
                                                         //this method os not executed at all

        if (!int.TryParse(values[routeKey]?.ToString(), out int countryId)) return false;

        return (countryId >= 1 && countryId <= 5);
    }
}
