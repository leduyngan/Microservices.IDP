using Microservice.IDP.Extensions;
using Microservice.IDP.Persistence;
using Microservice.IDP.Persistence.Migrations;
using Serilog;

Log.Information("Starting up");
var builder = WebApplication.CreateBuilder(args);

try
{
    builder.Host.AddAppConfigurations();
    builder.Host.ConfigureSerilog();

    var app = builder
        .ConfigureServices()
        .ConfigurePipeline();
    
    SeedUserData.EnsureSeedData(builder.Configuration.GetConnectionString("IdentitySqlConnection"));
    app
        .MigrateDatabase()
        .Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal)) throw;

    Log.Fatal(ex, $"Unhandled exception: {ex.Message}");
}
finally
{
    Log.Information($"Shutdown {builder.Environment.ApplicationName} complete");
    Log.CloseAndFlush();
}