using System;
using Microsoft.EntityFrameworkCore;
using NextOps.Api.Database;

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

   public static async Task MigrateDbAsync(this WebApplication app)
   {
      using var scope = app.Services.CreateScope();
      var dbContext = scope.ServiceProvider.GetRequiredService<NextOpsContext>();
      await dbContext.Database.MigrateAsync();
   }

   public static void MapEndpoints(this WebApplication app)
   {
      
   }

}
