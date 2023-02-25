using Microsoft.EntityFrameworkCore.PersistenceApi;

namespace Microsoft.EntityFrameworkCore.WebApplication
{
    public interface IOrderRepository : IEpaRepository<Order, Guid>
    {
    }
}
