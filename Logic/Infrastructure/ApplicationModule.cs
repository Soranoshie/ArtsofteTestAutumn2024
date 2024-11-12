using Logic.DAL;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Logic.Infrastructure;

public class ApplicationModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection services)
    {
        services.AddControllersWithViews().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
        services.AddDbContext<ApplicationDbContext>();

        return services;
    }
}