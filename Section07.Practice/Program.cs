using Microsoft.Extensions.FileProviders;
using Section07.Practice.Binders;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions() { WebRootPath = "webroot"});
builder.Services.AddControllers(opt => opt.ModelBinderProviders.Insert(0, new PersonBinderProvider()))
    .AddXmlSerializerFormatters();

//builder.WebHost.ConfigureKestrel(options =>
//options.ListenAnyIP(5000,
//    opt =>
//    {
//        opt.UseHttps();
//        opt.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
//    }));

var app = builder.Build();

app.MapControllers();

app.Run();
