using System;
using Microsoft.EntityFrameworkCore;
using NextOps.Api.Database;
using NextOps.Api.Entities;

namespace NextOps.Api.Extensions;

public static class ServiceCollectionExtensions
{
 public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string? connString = configuration.GetConnectionString("NextOps");

        services.AddDbContext<NextOpsContext>(options =>
            options.UseNpgsql(connString)
            .UseSnakeCaseNamingConvention()
            .UseSeeding(async (context, _) =>
            {
                if (!context.Set<Menu>().Any())
                {
                    context.Set<Menu>().AddRange(MenuSeed.Menu);
                    context.SaveChanges();
                }
            })
            .UseAsyncSeeding(async (context, _, cancellationToken) =>
            {
                if (!context.Set<Menu>().Any())
                {
                    context.Set<Menu>().AddRange(MenuSeed.Menu);
                    await context.SaveChangesAsync();
                }
            }));

        return services;
    }
}
