namespace Infrastructure.Data.Repository;

public abstract class EFBaseRepository<TKeyModel, TModel>
        where TModel : class
{
    private DbSet<TModel> _dbSet;
    private readonly DbContext _context;
    public EFBaseRepository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<TModel>();
    }


    #region Create 
    public virtual async Task CreateAsync(TModel model, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(model, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
    public virtual async Task CreatesArrangeAsync(IEnumerable<TModel> models, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(models);
        await _context.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region Update
    public virtual async Task UpdateAsync(TModel model)
    {
        await Task.Run(() => _dbSet.Update(model));
        await _context.SaveChangesAsync();
    }
    #endregion

    #region Get

    public async Task<TModel?> GetByIDAsync(TKeyModel id) => await _dbSet.FindAsync(id);
    public virtual IQueryable<TModel> QueryTracking() => _dbSet.AsQueryable();
    public virtual IQueryable<TModel> QueryNoTracking() => _dbSet.AsNoTracking().AsQueryable();
    public virtual Task<long> GetCountLongByAsync(CancellationToken cancellationToken = default) => _dbSet.LongCountAsync(cancellationToken);

    #endregion

    #region Get Count

    public virtual Task<int> GetCountAsync(CancellationToken cancellationToken = default) => _dbSet.CountAsync(cancellationToken);
    public virtual Task<long> GetCountLongAsync(CancellationToken cancellationToken = default) => _dbSet.LongCountAsync(cancellationToken);

    #endregion

    #region Delete
    public virtual async Task DeleteHardAsync(TModel model)
    {
        _dbSet.Remove(model);
        await _context.SaveChangesAsync();
    }
    public virtual async Task DeleteHardAsync(TKeyModel keyModel)
    {
        var model = await _dbSet.FindAsync(keyModel);
        if (model == null)
            return;
        _dbSet.Remove(model);
    }
    public virtual async Task DeletesHardAsync(IEnumerable<TModel> models)
    {
        _dbSet.RemoveRange(models);
    }
    public virtual async Task DeletesHardAsync(IEnumerable<TKeyModel> keys)
    {
        foreach (var key in keys)
        {
            var model = await _dbSet.FindAsync(key);
            if (model != null)
                _dbSet.Remove(model);
        }

        await _context.SaveChangesAsync();
    }
    public virtual async Task TruncateAsync()
    {
        string cmd = $"TRUNCATE TABLE {AnnotationHelper.TableName(_dbSet)}";
        await _context.Database.ExecuteSqlRawAsync(cmd);
    }



    public virtual Task DeleteSoftAsync(TModel model)
    {
        throw new NotImplementedException();
    }
    public virtual Task DeleteSoftAsync(TKeyModel keyModel)
    {
        throw new NotImplementedException();
    }
    public virtual Task DeletesSoftAsync(IEnumerable<TKeyModel> keys)
    {
        throw new NotImplementedException();
    }
    public virtual Task DeletesSoftAsync(IEnumerable<TModel> models)
    {
        throw new NotImplementedException();
    }
    public virtual Task DeleteAllSoftAsync()
    {
        throw new NotImplementedException();
    }

    #endregion

}