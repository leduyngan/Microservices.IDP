using Microservices.IDP.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Microservices.IDP.Persistence;

public class IdentityContext : IdentityDbContext<User>
{
    public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
    {
    }
    
    public DbSet<Permission> Permissions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // builder.ApplyConfiguration(new RoleConfiguration());
        builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        builder.ApplyIdentityConfiguration();
        // base.OnModelCreating(builder);
    }
}