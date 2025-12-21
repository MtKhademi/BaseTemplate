using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Infrastructure.Data;

namespace ECommerceModule.Persistence.Context;

internal class ECommerceUnitOfWork : IUnitOfWork
{
    private readonly ECommerceDbContext _dbContext;
    private IDbTransaction? _transaction;

    public ECommerceUnitOfWork(ECommerceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IDbTransaction> CreateTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadUncommitted)
    {
        if (_dbContext.Database.CurrentTransaction != null)
            return _dbContext.Database.CurrentTransaction.GetDbTransaction();

        var dbTransaction = await _dbContext.Database.BeginTransactionAsync(isolationLevel);
        _transaction = dbTransaction.GetDbTransaction();
        return _transaction;
    }

    public async Task CommitAsync()
    {
        if (_dbContext.Database.CurrentTransaction != null)
            await _dbContext.Database.CurrentTransaction.CommitAsync();
    }

    public async Task RollbackAsync()
    {
        if (_dbContext.Database.CurrentTransaction != null)
            await _dbContext.Database.CurrentTransaction.RollbackAsync();
    }

    public async Task SaveAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _dbContext.DisposeAsync();
    }
}
