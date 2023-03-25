namespace Microsoft.EntityFrameworkCore.PersistenceApi
{
    /// <summary>
    /// The interface for the paged list for <typeparam name="TEntity"></typeparam>
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public interface IPagedList<out TEntity> where TEntity : class
    {
        /// <summary>
        /// The total item count.
        /// </summary>
        public int TotalCount { get; }

        /// <summary>
        /// The total page size.
        /// </summary>
        public int TotalPages { get; }

        /// <summary>
        /// The current page number.
        /// </summary>
        public int CurrentPage { get; }

        /// <summary>
        /// The start page number.
        /// </summary>
        public int StartPage { get; }

        /// <summary>
        /// The end page number.
        /// </summary>
        public int EndPage { get; }

        /// <summary>
        /// The page size.
        /// </summary>
        public int PageSize { get; }

        public IReadOnlyCollection<TEntity> Items { get; }
    }
}
