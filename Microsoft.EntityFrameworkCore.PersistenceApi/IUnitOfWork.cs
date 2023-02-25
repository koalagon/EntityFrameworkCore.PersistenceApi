namespace Microsoft.EntityFrameworkCore.PersistenceApi;

/// <summary>
/// Represents the interface(s) for unit of work.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Gets the repository interface.
    /// </summary>
    /// <typeparam name="TRepository">The interface name. The interface should inherits <see cref="IEpaRepository{TEntity,TKey}"/></typeparam>
    /// <returns>The repository.</returns>
    TRepository GetRepository<TRepository>();

    /// <summary>
    /// Saves all changes made in this context to the underlying database.
    /// </summary>
    /// <returns>The number of state entries written to the underlying database. This can include state entries for entities and/or relationships. Relationship state entries are created for many-to-many relationships and relationships where there is no foreign key property included in the entity class (often referred to as independent associations).</returns>
    int SaveChanges();

    /// <summary>
    /// Asynchronously saves all changes made in this context to the underlying database.
    /// </summary>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the underlying database. This can include state entries for entities and/or relationships. Relationship state entries are created for many-to-many relationships and relationships where there is no foreign key property included in the entity class (often referred to as independent associations).</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}