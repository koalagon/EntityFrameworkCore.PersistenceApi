namespace Microsoft.EntityFrameworkCore.PersistenceApi
{
    /// <summary>
    /// The paged list for <typeparam name="TEntity"></typeparam>
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public class PagedList<TEntity> : IPagedList<TEntity> where TEntity : class
    {
        public PagedList(int totalCount, int pageSize, int pageNumber, IReadOnlyCollection<TEntity> items)
        {
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);
            CurrentPage = pageNumber;
            PageSize = pageSize;
            Items = items;
            var startPage = CurrentPage - 5;
            var endPage = CurrentPage + 4;
            if (startPage <= 0)
            {
                endPage -= (startPage - 1);
                startPage = 1;
            }

            if (endPage > TotalPages)
            {
                endPage = TotalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }

            StartPage = startPage;
            EndPage = endPage;
        }

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
