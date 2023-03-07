using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore.PersistenceApi;

/// <summary>
/// Represents an interface for generic repository.
/// </summary>
/// <typeparam name="TEntity">The entity.</typeparam>
/// <typeparam name="TKey">The entity key type. e.g. Guid, string, int</typeparam>
internal class EpaRepository<TEntity, TKey> : IEpaRepository<TEntity, TKey> where TEntity : class
{
    private readonly IEpaDbContext _dbContext;

    /// <summary>
    /// Constructs a new unit of work using <see cref="IEpaDbContext"/>.
    /// </summary>
    /// <param name="dbContext">Any DbContext should implements <see cref="IEpaDbContext"/>. The DbContext should be injected when the application starts.</param>
    public EpaRepository(IEpaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc cref="IEpaRepository{TEntity,TKey}.Add"/>
    public void Add(TEntity entity)
    {
        _dbContext.Set<TEntity>().Add(entity);
    }

    /// <inheritdoc cref="IEpaRepository{TEntity,TKey}.AddRange"/>
    public void AddRange(IEnumerable<TEntity> entities)
    {
        _dbContext.Set<TEntity>().AddRange(entities);
    }

    /// <inheritdoc cref="IEpaRepository{TEntity,TKey}.Remove(TEntity)"/>
    public void Remove(TEntity entity)
    {
        _dbContext.Set<TEntity>().Remove(entity);
    }

    /// <inheritdoc cref="IEpaRepository{TEntity,TKey}.RemoveRange"/>
    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        _dbContext.Set<TEntity>().RemoveRange(entities);
    }

    /// <inheritdoc cref="IEpaRepository{TEntity,TKey}.Update"/>
    public void Update(TEntity entity)
    {
        _dbContext.Set<TEntity>().Attach(entity);
        _dbContext.Entry(entity).State = EntityState.Modified;
    }

    /// <inheritdoc cref="IEpaRepository{TEntity,TKey}.GetById"/>
    public TEntity GetById(TKey key)
    {
        return _dbContext.Set<TEntity>().Find(key);
    }

    /// <inheritdoc cref="IEpaRepository{TEntity,TKey}.Get"/>
    public IReadOnlyCollection<TEntity> Get(Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "")
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        return orderBy != null ? orderBy(query).ToList() : query.ToList();
    }

    /// <inheritdoc cref="IEpaRepository{TEntity,TKey}.GetAsync"/>
    public async Task<IReadOnlyCollection<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "")
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        return orderBy != null ? await orderBy(query).ToListAsync() : await query.ToListAsync();
    }
}