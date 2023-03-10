using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore.PersistenceApi;

/// <summary>
/// Represents an interface for generic repository.
/// </summary>
/// <typeparam name="TEntity">The entity.</typeparam>
/// <typeparam name="TKey">The entity key type. e.g. Guid, string, int</typeparam>
public interface IEpaRepository<TEntity, in TKey> where TEntity : class
{
    /// <summary>
    /// Adds a new entity in the repository.
    /// </summary>
    void Add(TEntity entity);

    /// <summary>
    /// Adds new entities in the repository.
    /// </summary>
    void AddRange(IEnumerable<TEntity> entities);

    /// <summary>
    /// Removes the entity.
    /// </summary>
    void Remove(TEntity entity);

    /// <summary>
    /// Removes the entities.
    /// </summary>
    void RemoveRange(IEnumerable<TEntity> entities);

    /// <summary>
    /// Updates the entity.
    /// </summary>
    /// <remarks>
    /// Use this method when you know the entity exists in the database, but the entity is not retrieved yet.
    /// </remarks>
    void Update(TEntity entity);

    /// <summary>
    /// Gets a single entity with the given identifier.
    /// </summary>
    TEntity? GetById(TKey key);

    /// <summary>
    /// Returns a result set.
    /// </summary>
    IReadOnlyCollection<TEntity> Get(Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "", int? skip = null, int? take = null);

    /// <summary>
    /// Asynchronously returns a result set.
    /// </summary>
    Task<IReadOnlyCollection<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string includeProperties = "", int? skip = null, int? take = null);

    /// <summary>
    /// Returns single entity with the predicate.
    /// </summary>
    TEntity? SingleOrDefault(Expression<Func<TEntity, bool>>? filter = null, string includeProperties = "");

    /// <summary>
    /// Asynchronously returns single entity with the predicate.
    /// </summary>
    Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>>? filter = null, string includeProperties = "");

    /// <summary>
    /// Determines whether any element of a sequence exists or satisfies a condition.
    /// </summary>
    bool Any(Expression<Func<TEntity, bool>>? filter = null);

    /// <summary>
    /// Asynchronously determines whether any element of a sequence exists or satisfies a condition.
    /// </summary>
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? filter = null);
}