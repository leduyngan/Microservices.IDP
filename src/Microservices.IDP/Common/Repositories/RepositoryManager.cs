using Microservices.IDP.Common.Domains;
using Microservices.IDP.Common.Repositories.Interfaces;
using Microservices.IDP.Entities;
using Microservices.IDP.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microservices.IDP.Common.Repositories;

public class RepositoryManager : IRepositoryManager
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IdentityContext _dbContext;
    public UserManager<User> UserManager { get; }
    public RoleManager<IdentityRole> RoleManager { get; }
    private readonly Lazy<IPermissionRepository> _permissionRepository;

    public RepositoryManager(IUnitOfWork unitOfWork, IdentityContext dbContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _unitOfWork = unitOfWork;
        _dbContext = dbContext;
        UserManager = userManager;
        RoleManager = roleManager;
        _permissionRepository = new Lazy<IPermissionRepository>(() => new PermissionRepository(_dbContext, _unitOfWork));
    }

    public int ContextHashCode => _dbContext.GetHashCode(); // Thêm để kiểm tra instance

    public IPermissionRepository Permission => _permissionRepository.Value;

    public Task<int> SaveAsync()
        => _unitOfWork.CommitAsync();

    public Task<IDbContextTransaction> BeginTransactionAsync()
        => _dbContext.Database.BeginTransactionAsync();

    public Task EndTransactionAsync()
        => _dbContext.Database.CommitTransactionAsync();

    public void RollbackTransaction()
        => _dbContext.Database.RollbackTransactionAsync();
}