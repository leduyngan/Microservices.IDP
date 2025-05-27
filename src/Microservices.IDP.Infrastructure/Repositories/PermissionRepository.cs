using Dapper;
using Microservices.IDP.Infrastructure.Domains;
using Microservices.IDP.Infrastructure.Entities;
using Microservices.IDP.Infrastructure.Repositories.Interfaces;
using Microservices.IDP.Infrastructure.ViewModels;
using Microservices.IDP.Persistence;

namespace Microservices.IDP.Infrastructure.Repositories;

public class PermissionRepository : RepositoryBase<Permission, long>, IPermissionRepository
{
    public PermissionRepository(IdentityContext dbContext, IUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
    {
    }
    
    public async Task<IReadOnlyList<PermissionViewModel>> GetPermissionsByRole(string roleId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@roleId", roleId);
        var result = await QueryAsync<PermissionViewModel>("Get_Permissions_ByRoleId", parameters);
        return result;
    }
    
    // Tạo store procedure
    // Use [MicroserviceIdentity]
    // GO
    //     SET ANSI_NULLS ON
    // GO
    //     SET QUOTED_IDENTIFIER ON
    // GO
    //     CREATE PROCEDURE [Get_Permissions_ByRoleId]
    // @roleId varchar(50) NULL
    //     AS
    // BEGIN
    //     SET NOCOUNT ON;
    // SELECT * 
    //     FROM [Identity].Permissions WHERE RoleId = @roleId
    //     END


    public void UpdatePermissionsByRoleId(string roleId, IEnumerable<Permission> permissionCollection, bool trackChanges = false)
    {
        throw new NotImplementedException();
    }
}