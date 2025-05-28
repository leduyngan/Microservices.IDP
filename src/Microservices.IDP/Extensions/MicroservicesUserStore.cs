using Microservices.IDP.Infrastructure.Entities;
using Microservices.IDP.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Microservices.IDP.Extensions;

public class MicroservicesUserStore : UserStore<User, IdentityRole, IdentityContext>
{
    public MicroservicesUserStore(IdentityContext context, IdentityErrorDescriber describer = null) : base(context, describer)
    {
    }

    public override async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken = new CancellationToken())
    {
        var query = from userRole in Context.UserRoles
            join role in Context.Roles on userRole.RoleId equals role.Id
            where userRole.UserId.Equals(user.Id)
            select role.Id;  // Select role Id
        return await query.ToListAsync(cancellationToken);
    }
}