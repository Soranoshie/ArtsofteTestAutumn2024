using Logic.DAL;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Logic.Infrastructure;

public class ApplicationModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection services)
    {
        services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
        services.AddDbContext<ApplicationDbContext>();
        // services.AddAutoMapper(typeof(Program).Assembly);

        return services;
    }
}