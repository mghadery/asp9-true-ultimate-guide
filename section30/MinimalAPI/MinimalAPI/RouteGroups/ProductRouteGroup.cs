using MinimalAPI.Filters;
using MinimalAPI.Models;

namespace MinimalAPI.RouteGroups;

public static class ProductRouteGroup
{
    private static List<Product> products = [new Product(1, "Table"), new Product(2, "Chair")];

    public static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder builder)
    {
        builder.MapGet("", (HttpContext context) => products);

        builder.MapGet("{id:int}", (HttpContext context, int? id) =>
        {
            var p = products.FirstOrDefault(x => x.Id == id);
            if (p is null)
                return Results.NotFound();
            return Results.Ok(p);
        });

        builder.MapPost("", (HttpContext context, Product product) =>
        {
            products.Add(product);
            return Results.Ok(product);
        })
            .AddEndpointFilter<ProductValidationFilter>();


        builder.MapPut("{id:int}", (HttpContext context, int? id, Product product) =>
        {
            var p = products.FirstOrDefault(x => x.Id == id);
            if (p is null)
                return Results.NotFound();
            if (id != product.Id)
                return Results.BadRequest("unmatched Ids");

            p.Name = product.Name;
            return Results.Ok("Done");
        })
            .AddEndpointFilter<ProductValidationFilter>();

        builder.MapDelete("{id:int}", (HttpContext context, int? id) =>
        {
            var p = products.FirstOrDefault(x => x.Id == id);
            if (p is null)
                return Results.NotFound();

            products.Remove(p);
            return Results.Ok("Done");
        });
        return builder;
    }
}


