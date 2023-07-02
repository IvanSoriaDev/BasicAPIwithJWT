using BasicAPIwithJWT.Data;
using BasicAPIwithJWT.Services;
using Microsoft.EntityFrameworkCore;

namespace BasicAPIwithJWT.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddAplicationServices(this IServiceCollection services, IConfiguration config)
    {
        try
        {
            services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        });

            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
        catch (System.Exception)
        {

            throw;
        }
    }

}
