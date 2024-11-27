using Heritage.Application.DataShaping;
using Heritage.Application.DataTransferObjects;
using Heritage.Application.Interfaces;
using Heritage.Application.Services;
using Heritage.Domain;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Heritage.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.RegisterMapsterConfig();
        services.AddScoped<IArticleService, ArticleService>();
        services.AddScoped<IArticlePreviewService, ArticlePreviewService>();
        services.AddScoped<IDataShaper<ArticleDto>, DataShaper<ArticleDto>>();
        return services;
    }

    public static void RegisterMapsterConfig(this IServiceCollection _)
    {
        TypeAdapterConfig<Order, OrderDto>.NewConfig()
            .Map(dest => dest.UserName, _ => (string)MapContext.Current!.Parameters["username"]);

        TypeAdapterConfig<UpdateOrderDto, Order>.NewConfig()
            .Map(dest => dest.Id, _ => (Guid)MapContext.Current!.Parameters["id"])
            .Map(dest => dest.UserId, _ => (string)MapContext.Current!.Parameters["userId"]);
    }
}
