using Microservice.IDP.Common;
using Microservice.IDP.Entities;
using Microservice.IDP.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Microservice.IDP.Extensions;

public static class ServiceExtensions
{
    internal static void AddAppConfigurations(this ConfigureHostBuilder host)
    {
        host.ConfigureAppConfiguration((context, config) =>
        {
            var env = context.HostingEnvironment;
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        });
    }
    
    internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var emailSettings = configuration.GetSection(nameof(SMTPEmailSetting))
            .Get<SMTPEmailSetting>();
        services.AddSingleton(emailSettings);
        
        return services;
    }

    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
        });
    }
    
    public static void ConfigureSerilog(this ConfigureHostBuilder host)
    {
        host.UseSerilog((context, configuration) =>
        {
            var applicationName = context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-");
            var environmentName = context.HostingEnvironment.EnvironmentName ?? "Development";
            var elasticUri = context.Configuration.GetValue<string>("ElasticConfiguration:Uri");
            var username = context.Configuration.GetValue<string>("ElasticConfiguration:Username");
            var password = context.Configuration.GetValue<string>("ElasticConfiguration:Password");
            
            if(string.IsNullOrEmpty(elasticUri))
                throw new Exception("ElasticConfiguration Uri is not configured");

            configuration
                .WriteTo.Debug()
                .WriteTo.Console(outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
                {
                    // "microservicelogs-basket-api-development-2025-08"
                    IndexFormat = $"{applicationName}-{environmentName}-{DateTime.UtcNow:yyyy-MM}",
                    AutoRegisterTemplate = true,
                    NumberOfReplicas = 1,
                    NumberOfShards = 2,
                    ModifyConnectionSettings = x => x.BasicAuthentication(username, password)
                })
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Environment", environmentName)
                .Enrich.WithProperty("Application", applicationName)
                .ReadFrom.Configuration(context.Configuration);
        });
    }

    public static void ConfigureIdentityServer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("IdentitySqlConnection");
        services.AddIdentityServer(options =>
            {
                // https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/api_scopes#authorization-based-on-scopes
                options.EmitStaticAudienceClaim = true;
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
            // not recommended for production - you need to store your key material somewhere secure
            .AddDeveloperSigningCredential()
            // .AddInMemoryIdentityResources(Config.IdentityResources)
            // .AddInMemoryApiScopes(Config.ApiScopes)
            // .AddInMemoryClients(Config.Clients)
            // .AddInMemoryApiResources(Config.ApiResources)
            // .AddTestUsers(TestUsers.Users)
            .AddConfigurationStore(option =>
            {
                option.ConfigureDbContext = c => c.UseSqlServer(
                    connectionString,
                    builder => builder.MigrationsAssembly(typeof(Program).Assembly.FullName));
            })
            .AddOperationalStore(option =>
            {
                option.ConfigureDbContext = c => c.UseSqlServer(
                    connectionString,
                    builder => builder.MigrationsAssembly(typeof(Program).Assembly.FullName));
            })
            .AddAspNetIdentity<User>()
            .AddProfileService<IdentityProfileService>()
            ;
    }
    
    public static void ConfigureIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("IdentitySqlConnection");
        services
            .AddDbContext<IdentityContext>(options => options
                .UseSqlServer(connectionString))
            .AddIdentity<User, IdentityRole>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 6;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
                opt.User.RequireUniqueEmail = true;
                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                opt.Lockout.MaxFailedAccessAttempts = 3;
            })
            .AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders();
    }
}