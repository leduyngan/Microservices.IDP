using Microservices.IDP.Infrastructure.Domains;
using Microservices.IDP.Infrastructure.Entities;
using Microservices.IDP.Infrastructure.Repositories.Interfaces;
using Microservices.IDP.Persistence;

namespace Microservices.IDP.Infrastructure.Repositories;

public class PermissionRepository : RepositoryBase<Permission, long>, IPermissionRepository
{
    public PermissionRepository(IdentityContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
    {
    }
    
    public Task<IEnumerable<Permission>> GetPermissionsByRole(string roleId, bool trackChanges = false)
    {
        throw new NotImplementedException();
    }

    public void UpdatePermissionsByRoleId(string roleId, IEnumerable<Permission> permissionCollection, bool trackChanges = false)
    {
        throw new NotImplementedException();
    }
}