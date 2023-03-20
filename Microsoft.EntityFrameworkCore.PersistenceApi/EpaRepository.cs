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
    public TEntity? GetById(TKey key)
    {
        return _dbContext.Set<TEntity>().Find(key);
    }

    /// <inheritdoc cref="IEpaRepository{TEntity,TKey}.Get"/>
    public IReadOnlyCollection<TEntity> Get(Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "",
        int? skip = null, int? take = null)
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        if (orderBy != null)
        {
            query = orderBy(query);
            if (skip.HasValue) query = query.Skip(skip.Value);
            if (take.HasValue) query = query.Take(take.Value);

            return query.ToList();
        }

        return query.ToList();
    }

    /// <inheritdoc cref="IEpaRepository{TEntity,TKey}.GetAsync"/>
    public async Task<IReadOnlyCollection<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "",
        int? skip = null, int? take = null)
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        if (orderBy != null)
        {
            query = orderBy(query);
            if (skip.HasValue) query = query.Skip(skip.Value);
            if (take.HasValue) query = query.Take(take.Value);

            return await query.ToListAsync();
        }
        
        return await query.ToListAsync();
    }

    /// <inheritdoc cref="IEpaRepository{TEntity,TKey}.SingleOrDefault"/>
    public TEntity? SingleOrDefault(Expression<Func<TEntity, bool>>? filter = null, string includeProperties = "")
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        return query.SingleOrDefault();
    }

    /// <inheritdoc cref="IEpaRepository{TEntity,TKey}.SingleOrDefaultAsync"/>
    public async Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null, string includeProperties = "")
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        return await query.SingleOrDefaultAsync();
    }

    /// <inheritdoc cref="IEpaRepository{TEntity,TKey}.Any"/>
    public bool Any(Expression<Func<TEntity, bool>> filter) => _dbContext.Set<TEntity>().Any(filter);

    /// <inheritdoc cref="IEpaRepository{TEntity,TKey}.AnyAsync"/>
    public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter) => _dbContext.Set<TEntity>().AnyAsync(filter);

    /// <inheritdoc cref="IEpaRepository{TEntity,TKey}.All"/>
    public bool All(Expression<Func<TEntity, bool>> filter) => _dbContext.Set<TEntity>().All(filter);

    /// <inheritdoc cref="IEpaRepository{TEntity,TKey}.AllAsync"/>
    public Task<bool> AllAsync(Expression<Func<TEntity, bool>> filter) => _dbContext.Set<TEntity>().AllAsync(filter);
}