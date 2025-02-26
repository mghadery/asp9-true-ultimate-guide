using Autofac;
using Autofac.Extensions.DependencyInjection;
using Section12.Practice.IService;
using Section12.Practice.Service;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

//Autofac
//builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
//builder.Host.ConfigureContainer<ContainerBuilder>(    
//    cb =>
//    cb.RegisterType<CitiesService>().As<ICitiesService>().InstancePerLifetimeScope());

//default DI
//builder.Services.Add(new ServiceDescriptor(typeof(ICitiesService),
//    typeof(CitiesService),
//    ServiceLifetime.Scoped
//   ));
var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
