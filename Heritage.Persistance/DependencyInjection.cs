using Heritage.Application.Interfaces;
using Heritage.Application.Services;
using Heritage.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Heritage.Persistance;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistance(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PostgreSQL");
        services.AddDbContext<HeritageDbContext>(options =>
        {
            options.UseNpgsql(connectionString, options => options.MigrationsAssembly("Heritage.WebApi"));
        });
        services.AddScoped<IArticleRepository, ArticleRepository>();
        services.AddScoped<IArticlePreviewRepository, ArticlePreviewRepository>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IRoomsService, RoomsService>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderService, OrderService>();
        return services;
    }
}
