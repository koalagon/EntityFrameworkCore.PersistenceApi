using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.PersistenceApi;

namespace Microsoft.EntityFrameworkCore.WebApplication.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public IReadOnlyCollection<Order> FilteredOrders;
        public IReadOnlyCollection<Order> AllOrders;
        public IReadOnlyCollection<Order> SkipAndTakeOrders;
        public IPagedList<Order> PagedList;
        public Order? Order;


        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task OnGetAsync(int? pageNum = 1)
        {
            var orderRepository = _unitOfWork.GetRepository<IOrderRepository>();
            FilteredOrders = await _unitOfWork.Queryable<Order>().Where(OrderFilters.NameFilter).OrderByDescending(b => b.AddedAt).ToListAsync();
            AllOrders = await _unitOfWork.Queryable<Order>().ToListAsync();
            SkipAndTakeOrders = await orderRepository.GetAsync(o => !o.IsDeleted, skip: 1, take: 2);
            Order = _unitOfWork.Queryable<Order>().FirstOrDefault(q => q.Name.Contains("World"));
            PagedList = await _unitOfWork.Queryable<Order>().OrderBy(o => o.Name).ToPagedListAsync(pageNum.Value, 5);
        }

        public async Task<IActionResult> OnPostAddAsync(string name)
        {
            _unitOfWork.GetRepository<IOrderRepository>().Add(new Order(name));
            await _unitOfWork.SaveChangesAsync();

            return Redirect("./Index");
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var orderRepository = _unitOfWork.GetRepository<IOrderRepository>();
            var order = orderRepository.GetById(id);
            orderRepository.Remove(order);
            await _unitOfWork.SaveChangesAsync();

            return Redirect("./Index");
        }

        public async Task<IActionResult> OnPostSoftDeleteAsync(Guid id)
        {
            var orderRepository = _unitOfWork.GetRepository<IOrderRepository>();
            var order = orderRepository.GetById(id);
            order.IsDeleted = true;
            order.DeletedAt = DateTime.UtcNow;
            await _unitOfWork.SaveChangesAsync();

            return Redirect("./Index");
        }
    }
}