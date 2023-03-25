using System.Linq.Dynamic.Core;

namespace Microsoft.EntityFrameworkCore.PersistenceApi
{
    /// <summary>
    /// Extension method for <see cref="IQueryable"/>
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Returns the paged list for the entity.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="queryable"><see cref="IQueryable"/></param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns><see cref="IPagedList{TEntity}"/></returns>
        public static IPagedList<TEntity> ToPagedList<TEntity>(this IQueryable<TEntity> queryable, int pageNumber = 1, int pageSize = 25) where TEntity : class
        {
            queryable = typeof(TEntity).IsAssignableTo(typeof(IDeletable)) ? queryable.Where($"{nameof(IDeletable.IsDeleted)} == false") : queryable;
            var totalItems = queryable.Count();
            var items = queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedList<TEntity>(totalItems, pageSize, pageNumber, items);
        }

        /// <summary>
        /// Asynchronously returns the paged list for the entity.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="queryable"><see cref="IQueryable"/></param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns><see cref="IPagedList{TEntity}"/></returns>
        public static async Task<IPagedList<TEntity>> ToPagedListAsync<TEntity>(this IQueryable<TEntity> queryable, int pageNumber = 1, int pageSize = 25, CancellationToken cancellationToken = default) where TEntity : class
        {
            queryable = typeof(TEntity).IsAssignableTo(typeof(IDeletable)) ? queryable.Where($"{nameof(IDeletable.IsDeleted)} == false") : queryable;
            var totalItems = await queryable.CountAsync(cancellationToken);
            var items = await queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return new PagedList<TEntity>(totalItems, pageSize, pageNumber, items);
        }
    }
}
