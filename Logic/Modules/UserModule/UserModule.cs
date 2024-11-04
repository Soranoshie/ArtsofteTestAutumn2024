using Logic.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Logic.Modules.UserModule;

public class UserModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<AuthService>();

        return services;
    }
}