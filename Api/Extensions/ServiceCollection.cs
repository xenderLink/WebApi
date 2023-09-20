using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Repositories;

namespace Store.Extensions;

public static class ServiceCollection
{
    public static void RegisterServices(this IServiceCollection Services, WebApplicationBuilder builder)
    {
        Services.AddDbContext<StoreDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("Store")));

        Services.Configure<RouteOptions>(routeOptions => 
            {
                routeOptions.ConstraintMap.Add("short", typeof(ShortConstraint));            
            });

        Services.AddScoped<IProductRepository, ProductRepository>();
        Services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
    }        
}