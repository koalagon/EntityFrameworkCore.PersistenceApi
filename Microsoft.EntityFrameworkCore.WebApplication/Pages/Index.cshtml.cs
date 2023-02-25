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
        public Order? Order;


        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task OnGetAsync()
        {
            var orderRepository = _unitOfWork.GetRepository<IOrderRepository>();
            FilteredOrders = await orderRepository.GetAsync(filter: OrderFilters.NameFilter, orderBy: r => r.OrderByDescending(b => b.AddedAt));
            AllOrders = await orderRepository.GetAsync();
            Order = _unitOfWork.GetRepository<IOrderRepository>().Queryable(q => q.Name.Contains("World")).FirstOrDefault();
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
    }
}