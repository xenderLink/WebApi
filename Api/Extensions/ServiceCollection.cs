using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Services;
using Store.Services.Repositories;

namespace Store.Extensions;

public static class ServiceCollection
{
    public static void RegisterServices(this IServiceCollection Services, WebApplicationBuilder builder)
    {
        Services.AddControllers();

        Services.AddDbContext<StoreDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("Store")));

        Services.Configure<RouteOptions>(routeOptions => 
            {
                routeOptions.ConstraintMap.Add("short", typeof(ShortConstraint));            
            });

        Services.AddEndpointsApiExplorer();
        Services.AddSwaggerGen();
        Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        Services.AddScoped<IProductRepository, ProductRepository>();
        Services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
        
        Services.AddScoped<IMessageProducer, MessageProducer>();
    }        
}