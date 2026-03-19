using Microsoft.AspNetCore.Identity;
using NextOps.Api.Database;
using NextOps.Api.Extensions;
using NextOps.Api.Entities;

var builder = WebApplication.CreateBuilder(args);

string[]? allowedOrigins = builder.Configuration
    .GetSection("AllowedOrigins")
    .Get<string[]>() ?? Array.Empty<string>();

builder.Services.AddDatabase(builder.Configuration);



builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<NextOpsContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigins",
                          policy =>
                          {
                              policy.WithOrigins(allowedOrigins)
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod()
                                                  .AllowCredentials();
                          });
});

var app = builder.Build();
app.MapIdentityApi<ApplicationUser>().WithTags("Authentication");

// Map application endpoints
app.MapEndpoints();

await app.MigrateDbAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithUi();
}

app.UseHttpsRedirection();

app.UseCors("AllowOrigins");
app.Run();