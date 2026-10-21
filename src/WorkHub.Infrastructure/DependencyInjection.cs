using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkHub.Application.Common.Interfaces;
using WorkHub.Infrastructure.Persistence;
using WorkHub.Infrastructure.Repositories;

namespace WorkHub.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("DefaultConnection bulunamadı.");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>()); //savechanges zaten dbcontext den geliyor
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
