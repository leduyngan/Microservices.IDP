using Microservice.IDP.Entities;
using Microservice.IDP.Entities.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Microservice.IDP.Persistence;

public class IdentityContext : IdentityDbContext<User>
{
    public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // builder.ApplyConfiguration(new RoleConfiguration());
        builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        builder.ApplyIdentityConfiguration();
        // base.OnModelCreating(builder);
    }
}