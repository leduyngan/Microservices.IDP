using System.Security.Claims;
using IdentityModel;
using Microservices.IDP.Common;
using Microservices.IDP.Infrastructure.Common;
using Microservices.IDP.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Microservices.IDP.Persistence.Migrations;

public class SeedUserData
{
    public static void EnsureSeedData(string connectionString)
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDbContext<IdentityContext>(options => options.UseSqlServer(connectionString));

        services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 6;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders();

        using (var serviceProvider = services.BuildServiceProvider())
        {
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                CreateUser(scope, "Alice", "Smith", "Alice Smith's Wollongong", Guid.NewGuid().ToString(), "alice123", "Administrator", "alicesmith@example.com" );
            }
        }
    }

    private static void CreateUser(IServiceScope scope, string firstName, string lastName, string address, string id, string password,
         string role, string email)
    {
        var userManagement = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var user = userManagement.FindByEmailAsync(email).Result;
        if (user == null)
        {
            user = new User
            {
                Id = id,
                UserName = email,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Address = address,
                EmailConfirmed = true,
            };
            var result = userManagement.CreateAsync(user, password).Result; 
            CheckResult(result);
            
            var addToRoleResult = userManagement.AddToRoleAsync(user, role).Result;
            CheckResult(addToRoleResult);
            
            result = userManagement.AddClaimsAsync(user, new Claim[]
            {
                new(SystemConstants.Claims.UserName, user.UserName),
                new(SystemConstants.Claims.FirstName, user.FirstName),
                new(SystemConstants.Claims.LastName, user.LastName),
                new(SystemConstants.Claims.Roles, role),
                new(JwtClaimTypes.Address, user.Address),
                new(JwtClaimTypes.Email, user.Email), 
                new(ClaimTypes.NameIdentifier, user.Id)
            }).Result;
            CheckResult(result);
        }
    }

    private static void CheckResult(IdentityResult result)
    {
        if (!result.Succeeded)
        {
            throw new Exception(result.Errors.First().Description);
        }
    }
}