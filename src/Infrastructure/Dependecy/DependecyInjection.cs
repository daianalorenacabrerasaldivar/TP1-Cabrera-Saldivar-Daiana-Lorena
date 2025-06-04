using Application.Common.Interface.Infrastructure;
using Infrastructure.Persistencia;
using Infrastructure.Persistencia.Context;
using Infrastructure.Persistencia.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Dependecy
{
    public static class DependecyInjection
    {
        public static void AddDependecyInjectionInfrastructure(this IServiceCollection services)
        {
            // Registrar ApplicationDbContext
            services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddScoped<DbContext>(provider => provider.GetService<ApplicationDbContext>());
            services.AddScoped<IDesignTimeDbContextFactory<ApplicationDbContext>, ApplicationDbContextFactory>();

            services.AddScoped<IRepositoryCommand, RepositoryCommand>();
            services.AddScoped<IRepositoryQuery>(provider =>
        {
            var dbContext = provider.GetRequiredService<ApplicationDbContext>();
            return new RepositoryQuery<ApplicationDbContext>(dbContext);
        });

        }
    }
}
