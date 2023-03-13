using Castle.DynamicProxy;
using System.Collections;
using System.Linq.Dynamic.Core;

namespace Microsoft.EntityFrameworkCore.PersistenceApi;

/// <summary>
/// Represents the default implementation of the <see cref="IUnitOfWork"/> interface.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly IEpaDbContext _dbContext;
    private readonly Hashtable _repositories;
    private readonly ProxyGenerator _proxyGenerator = new();

    /// <summary>
    /// Constructs a new unit of work using <see cref="IEpaDbContext"/>.
    /// </summary>
    /// <param name="dbContext">Any DbContext should implements <see cref="IEpaDbContext"/>. The DbContext should be injected when the application starts.</param>
    public UnitOfWork(IEpaDbContext dbContext)
    {
        _dbContext = dbContext;
        _repositories ??= new Hashtable();

        AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes())
            .Where(p => p.IsInterface && p.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEpaRepository<,>)))
            .ToList().ForEach(c =>
            {
                var proxy = _proxyGenerator.CreateInterfaceProxyWithoutTarget(c, new EpaRepositoryInterceptor(_dbContext));
                if (!_repositories.ContainsKey(c))
                {
                    _repositories.Add(c, proxy);
                }
            });
    }

    /// <summary>
    /// Gets the repository interface.
    /// </summary>
    /// <typeparam name="TRepository">The interface name. The interface should inherits <see cref="IEpaRepository{TEntity,TKey}"/></typeparam>
    /// <returns>The repository.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the repository is not registered in the unit of work.</exception>
    public TRepository GetRepository<TRepository>()
    {
        var type = typeof(TRepository);

        if (!_repositories.ContainsKey(type))
        {
            throw new InvalidOperationException($"The {typeof(TRepository)} is not registered. Does your interface inherit IEpaRepository?");
        }

        return (TRepository)_repositories[type]!;
    }

    /// <inheritdoc cref="IEpaDbContext.SaveChanges"/>
    public int SaveChanges() => _dbContext.SaveChanges();

    /// <inheritdoc cref="IEpaDbContext.SaveChangesAsync"/>
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _dbContext.SaveChangesAsync(cancellationToken);

    /// <inheritdoc cref="IUnitOfWork.Queryable{TEntity}"/>
    public IQueryable<TEntity> Queryable<TEntity>() where TEntity : class
    {
        var queryable = _dbContext.Set<TEntity>().AsNoTracking().AsQueryable();
        return typeof(TEntity).IsAssignableTo(typeof(IDeletable)) ? queryable.Where($"{nameof(IDeletable.IsDeleted)} == false") : queryable;
    }

    #region Dispose
    private bool _disposed;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    #endregion
}