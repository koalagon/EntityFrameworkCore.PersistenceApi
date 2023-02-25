using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;

namespace Microsoft.EntityFrameworkCore.PersistenceApi;

/// <summary>
/// Represents an interface that any application DbContext should implement.
/// </summary>
public interface IEpaDbContext : IDisposable
{
    /// <summary>
    /// Returns a DbSet&lt;TEntity&gt; instance for access to entities of the given type in the context and the underlying store.
    /// </summary>
    /// <typeparam name="TEntity">The type entity for which a set should be returned.</typeparam>
    /// <returns>DbSet&lt;TEntity&gt;</returns>
    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    /// <summary>
    /// Gets a DbEntityEntry&lt;TEntity&gt; object for the given entity providing access to information about the entity and the ability to perform actions on the entity.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="entity">The entity.</param>
    /// <returns>DbEntityEntry&lt;TEntity&gt;</returns>
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

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