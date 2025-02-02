using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DAL;

public static class DI
{
    public static void AddDAL(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<OrdersTaskContext>(ctx =>
        {
            ctx.UseNpgsql(connectionString);
        });
    }
}
