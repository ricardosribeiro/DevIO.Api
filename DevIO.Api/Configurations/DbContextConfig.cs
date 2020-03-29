using DevIO.Api.Data;
using DevIO.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevIO.Api.Configurations
{
    public static class DbContextConfig
    {
        public static IServiceCollection AddDefaultDbContextConfiguration(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddDbContext<MeuDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Development"));
            });

            return services;
        }

        public static IServiceCollection AddIdentityDbContextConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Development"));
            });

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddRoles<IdentityRole>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
