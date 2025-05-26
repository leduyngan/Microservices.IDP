using Microservices.IDP.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microservices.IDP.Common.Repositories.Interfaces;

public interface IRepositoryManager
{
    // UserManager<User> UserManager { get; }
    // RoleManager<User> RoleManager { get; }
    IPermissionRepository Permission { get; }
    Task<int> SaveAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task EndTransactionAsync();
    void RollbackTransaction();
}
