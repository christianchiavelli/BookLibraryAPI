using Asp.Versioning;
using BookLibraryAPI.Data;
using BookLibraryAPI.Extensions.Authentication;
using BookLibraryAPI.Extensions.Database;
using BookLibraryAPI.Extensions.HealthChecks;
using BookLibraryAPI.Extensions.Swagger;
using BookLibraryAPI.Extensions.Logging;
using BookLibraryAPI.Extensions.ApiVersioning;
using BookLibraryAPI.Extensions.Routing;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Logging Configuration
builder.Services.AddSerilogLogging(builder.Configuration);
builder.Host.UseSerilogLogging();

// Authentication and Identity Configuration
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddIdentityConfiguration();

// Database Configuration
builder.Services.AddDatabaseConfiguration(builder.Configuration);

// API and Routing Configuration
builder.Services.AddApiVersioningConfiguration();
builder.Services.AddLowerCaseUrls();

// Health Checks Configuration
builder.Services.AddCustomHealthChecks();

// Swagger Configuration
builder.Services.AddSwaggerConfiguration();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var descriptions = app.DescribeApiVersions();
        foreach (var description in descriptions)
        {
            var url = $"/swagger/{description.GroupName}/swagger.json";
            var name = description.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }
    });
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");

app.MapControllers();

app.Run();
