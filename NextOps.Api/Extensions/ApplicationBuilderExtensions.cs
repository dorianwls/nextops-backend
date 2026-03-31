using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using NextOps.Api.Database;
using NextOps.Api.Endpoints;
using NextOps.Api.Endpoints.Users;


namespace NextOps.Api.Extensions;

public static class ApplicationBuilderExtensions
{
   public static IApplicationBuilder UseSwaggerWithUi(this WebApplication app)
   {
      app.UseSwagger();

      app.UseSwaggerUI(options =>
      {
         options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
         options.RoutePrefix = string.Empty;
      });

      return app;
   }

// Hecho con IA, no lo entiendo tengo que revisarlo luego, metodo para agregar bearer de seguridad en swagger para pruebas
public static IServiceCollection AddSwaggerWithSecurity(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "NextOps API",
                Version = "v1",
                Description = "API de NextOps"
            });

            // Definición del esquema Bearer
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Ingrese el token en este formato: Bearer {tu_token}",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT"
            });

            // ✅ Forma correcta en Swashbuckle 10 / .NET 10
            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
            });
        });

        return services;
    }

   public static async Task MigrateDbAsync(this WebApplication app)
   {
      using var scope = app.Services.CreateScope();
      var services = scope.ServiceProvider;
      try
      {
         var context = services.GetRequiredService<NextOpsContext>();
         var logger = services.GetRequiredService<ILogger<Program>>();

         logger.LogInformation("Iniciando migración de la base de datos...");

         await context.Database.MigrateAsync();

         logger.LogInformation("Migración de la base de datos completada exitosamente.");
      }
      catch (Exception ex)
      {
         var logger = services.GetRequiredService<ILogger<Program>>();
         logger.LogError(ex, "Ocurrió un error al aplicar las migraciones de la base de datos.");
               
         if (app.Environment.IsDevelopment())
         {
            throw;
         }
      }
   }

   public static void MapEndpoints(this WebApplication app)
   {
      app.MapUserEndpoints();
      app.MapProductsEndpoint();
   }




}
