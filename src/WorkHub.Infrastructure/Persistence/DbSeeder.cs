using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using WorkHub.Application.Common.Interfaces;
using WorkHub.Domain.Entities;
using WorkHub.Domain.Enums;

namespace WorkHub.Infrastructure.Persistence
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(
            IServiceProvider services, CancellationToken cancellationToken = default)
        {
            var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("DbSeeder");
            var configuration = services.GetRequiredService<IConfiguration>();

            var email = configuration["Seed:AdminEmail"];
            var password = configuration["Seed:AdminPassword"];

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                logger.LogInformation("Seed:AdminEmail / Seed:AdminPassword tanımlı değil, admin seed atlandı.");
                return;
            }

            var users = services.GetRequiredService<IUserRepository>();

            if (await users.EmailExistsAsync(email, cancellationToken))
                return;

            var hasher = services.GetRequiredService<IPasswordHasher>();
            var unitOfWork = services.GetRequiredService<IUnitOfWork>();

            var admin = User.Create(email, "System Admin", hasher.Hash(password), UserRole.Admin);

            await users.AddAsync(admin, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation("İlk admin kullanıcı oluşturuldu: {Email}", admin.Email);
        }
    }
}
