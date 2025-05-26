using Microservices.IDP.Common.Domains;
using Microservices.IDP.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microservices.IDP.Repositories;

public class RepositoryManager : IRepositoryManager
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IdentityContext _dbContext;
    // public UserManager<User> UserManager { get; }
    // public RoleManager<User> RoleManager { get; }

    public RepositoryManager(IUnitOfWork unitOfWork, IdentityContext dbContext)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        // UserManager = userManager;
        // RoleManager = roleManager;
    }

    public int ContextHashCode => _dbContext.GetHashCode(); // Thêm để kiểm tra instance
    
    public Task<int> SaveAsync()
        => _unitOfWork.CommitAsync();

    public Task<IDbContextTransaction> BeginTransactionAsync()
        => _dbContext.Database.BeginTransactionAsync();

    public Task EndTransactionAsync()
        => _dbContext.Database.CommitTransactionAsync();

    public void RollbackTransaction()
        => _dbContext.Database.RollbackTransactionAsync();
}