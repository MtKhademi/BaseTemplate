namespace Infrastructure.Data.Repository
{

    public interface IBaseRepository { }

    public interface ICRUDRepository<TKeyType, TEntity> :
        IBaseRepository,

        IGetsNoTrackingRepository<TEntity>,
        IGetsTrackingRepository<TEntity>,
        IGetByIdRepository<TKeyType, TEntity>,

        ICreateRepository<TEntity>,
        ICreatesRepository<TEntity>,

        IGetCountRepository,

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
    public interface IGetsNoTrackingRepository<TModel>
      where TModel : class
    {
        IQueryable<TModel> GetsQueryableNoTracker();
    }

    public interface IGetsModelNoTrackingRepository<TModel> where TModel : class
    {
        IQueryable<TModel> GetsModelQueryableNoTracker();
    }
    public interface IGetsModelNoTrackingRepository<TModel, TFilterModel>
        where TModel : class
        where TFilterModel : class
    {
        IQueryable<TModel> GetsModelQueryableNoTracker(TFilterModel filterModel);
    }

    public interface IGetsTrackingRepository<TModel>
        where TModel : class
    {
        IQueryable<TModel> GetsQueryableTracker();
    }
    public interface IGetByIdRepository<TKeyType, TModel>
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

    #endregion

    #region Create Repository

    public interface ICreateRepository<TModel>
       where TModel : class
    {
        Task CreateAsync(TModel model, CancellationToken cancellationToken = default);
    }
    public interface ICreatesRepository<TModel>
     where TModel : class
    {
        Task CreatesArrangeAsync(IEnumerable<TModel> models, CancellationToken cancellationToken = default);
    }


    #endregion

    #region Update Repository

    public interface IUpdateRepository<TModel>
    where TModel : class
    {
        Task UpdateAsync(TModel model);
    }

    #endregion

    #region Delete

    public interface IDeleteHardRepository<TKeyModel, TModel>
       where TModel : class
    {
        Task DeleteHardAsync(TModel model);
        Task DeleteHardAsync(TKeyModel keyModel);
    }
    public interface IDeletesHardRepository<TKeyModel, TModel>
       where TModel : class
    {
        Task DeletesHardAsync(IEnumerable<TKeyModel> keys);
        Task DeletesHardAsync(IEnumerable<TModel> models);
    }
    public interface ITruncateRepository
    {
        Task TruncateAsync();
    }


    public interface IDeleteSoftRepository<TKeyModel, TModel>
        where TModel : class
    {
        Task DeleteSoftAsync(TModel model);
        Task DeleteSoftAsync(TKeyModel keyModel);
    }
    public interface IDeletesSoftepository<TKeyModel, TModel>
       where TModel : class
    {
        Task DeletesSoftAsync(IEnumerable<TKeyModel> keys);
        Task DeletesSoftAsync(IEnumerable<TModel> models);
    }


    #endregion

}
