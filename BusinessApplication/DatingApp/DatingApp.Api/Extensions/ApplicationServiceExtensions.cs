using DatingApp.Api.Data;
using DatingApp.Api.Helpers;
using DatingApp.Api.Interfaces;
using DatingApp.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.Api.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            return services;
        }
    }
}
