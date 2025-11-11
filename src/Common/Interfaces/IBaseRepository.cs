namespace Common.Interfaces;


public interface IBaseRepository { }

public interface ICRUDRepository<TKeyType, TEntity> :
    IBaseRepository,
    IGetsNoTrackingRepository<TEntity>,
    IGetsTrackingRepository<TEntity>,
    IGetByIdRepository<TKeyType, TEntity>,

    IGetCountLongRepository,
    IGetCountRepository,

    ICreateRepository<TEntity>,
    ICreatesRepository<TEntity>,

    IUpdateRepository<TEntity>,

    IDeleteHardRepository<TKeyType, TEntity>,
    IDeletesHardRepository<TKeyType, TEntity>,
    ITruncateRepository,

    IDeletesSoftepository<TKeyType, TEntity>,
    IDeleteSoftRepository<TKeyType, TEntity>
    where TEntity : class
{

}

#region Get

public interface IGetsNoTrackingRepository<TModel> : IBaseRepository
  where TModel : class
{
    IQueryable<TModel> GetsQueryableNoTracker();
}
public interface IGetsModelNoTrackingRepository<TModel> : IBaseRepository where TModel : class
{
    IQueryable<TModel> GetsModelQueryableNoTracker();
}
public interface IGetsModelNoTrackingRepository<TModel, TFilterModel> : IBaseRepository
    where TModel : class
    where TFilterModel : class
{
    IQueryable<TModel> GetsModelQueryableNoTracker(TFilterModel filterModel);
}
public interface IGetsTrackingRepository<TModel> : IBaseRepository
    where TModel : class
{
    IQueryable<TModel> GetsQueryableTracker();
}
public interface IGetByIdRepository<TKeyType, TModel> : IBaseRepository
       where TModel : class
{
    Task<TModel?> GetByIDAsync(TKeyType id);
}

#endregion

#region Get Count
public interface IGetCountRepository<TFilter>
{
    Task<int> GetCountByAsync(TFilter filter);
}
public interface IGetCountRepository
{
    Task<int> GetCountAsync(CancellationToken cancellationToken = default);
}

public interface IGetCountLongRepository<TFilter>
{
    Task<long> GetCountLongByAsync(TFilter filter);
}
public interface IGetCountLongRepository
{
    Task<long> GetCountLongAsync(CancellationToken cancellationToken = default);
}
#endregion

#region Create Repository

public interface ICreateRepository<TModel> : IBaseRepository
   where TModel : class
{
    Task CreateAsync(TModel model);
}
public interface ICreatesRepository<TModel> : IBaseRepository
 where TModel : class
{
    Task CreatesArrangeAsync(IEnumerable<TModel> models);
}


#endregion

#region Update Repository

public interface IUpdateRepository<TModel> : IBaseRepository
where TModel : class
{
    Task UpdateAsync(TModel model);
}

#endregion

#region Delete

public interface IDeleteHardRepository<TKeyModel, TModel> : IBaseRepository
   where TModel : class
{
    Task DeleteHardAsync(TModel model);
    Task DeleteHardAsync(TKeyModel keyModel);
}
public interface IDeletesHardRepository<TKeyModel, TModel> : IBaseRepository
   where TModel : class
{
    Task DeletesHardAsync(IEnumerable<TKeyModel> keys);
    Task DeletesHardAsync(IEnumerable<TModel> models);
}
public interface ITruncateRepository : IBaseRepository
{
    Task TruncateAsync();
}


public interface IDeleteSoftRepository<TKeyModel, TModel> : IBaseRepository
    where TModel : class
{
    Task DeleteSoftAsync(TModel model);
    Task DeleteSoftAsync(TKeyModel keyModel);
}
public interface IDeletesSoftepository<TKeyModel, TModel> : IBaseRepository
   where TModel : class
{
    Task DeletesSoftAsync(IEnumerable<TKeyModel> keys);
    Task DeletesSoftAsync(IEnumerable<TModel> models);
}


#endregion
