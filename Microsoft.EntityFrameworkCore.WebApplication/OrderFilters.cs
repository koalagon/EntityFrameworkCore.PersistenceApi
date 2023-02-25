using System.Linq.Expressions;

namespace Microsoft.EntityFrameworkCore.WebApplication
{
    public static class OrderFilters
    {
        public static Expression<Func<Order, bool>> NameFilter = order => order.Name.Contains("Hello");
    }
}
