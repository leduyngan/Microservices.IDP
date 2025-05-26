using Microservices.IDP.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Microservices.IDP.Common.Domains;

public class UnitOfWork : IUnitOfWork
{
    private readonly IdentityContext _context;

    public UnitOfWork(IdentityContext context)
    {
        _context = context;
    }
    
    public int ContextHashCode => _context.GetHashCode(); // Thêm để kiểm tra instance
    
    public void Dispose()
    {
        _context.Dispose();
    }

    public Task<int> CommitAsync()
    {
        return _context.SaveChangesAsync();
    }
}