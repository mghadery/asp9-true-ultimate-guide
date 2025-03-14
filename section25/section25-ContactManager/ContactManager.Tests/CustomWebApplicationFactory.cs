using ContactManager.Infrastructure.Persistent.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace People.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.UseEnvironment("Test"); //used in program.cs

        builder.ConfigureServices(services =>
        {
            //var des = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<AppDbContext>));
            //if (des is not null) services.Remove(des);


            //des = services.SingleOrDefault(x => x.ServiceType == typeof(AppDbContext));
            //if (des is not null) services.Remove(des);

            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("people"));
        });
    }
}
