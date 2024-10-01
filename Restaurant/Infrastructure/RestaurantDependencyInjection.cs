using Application.Abstractions.Data;
using Application.Abstractions.Repositories;
using Infrastructure.Persistence.DataContext;
using Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class RestaurantDependencyInjection
{
    
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        
        services.AddSingleton<IDapperDbContext, DapperDbContext>(serviceProvider =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            var conectionString = configuration.GetConnectionString("Dapper");
            
            return new DapperDbContext(conectionString!);
        });

        services.AddScoped<IRoomTableRepository, RoomTableRepository>();
        services.AddScoped<IWaiterRepository, WaiterRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();

        return services;
    }
}
