using DAL;
using Services;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connectionString = builder.Configuration.GetValue<string>("ConnectionString")!;

            builder.Services.AddDAL(connectionString);
            builder.Services.AddServices();

            builder.Services.AddControllers();

            var app = builder.Build();
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}