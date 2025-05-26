using Microsoft.EntityFrameworkCore.Storage;

namespace Microservices.IDP.Repositories;

public interface IRepositoryManager
{
    // UserManager<User> UserManager { get; }
    // RoleManager<User> RoleManager { get; }
    
    Task<int> SaveAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task EndTransactionAsync();
    void RollbackTransaction();
}
