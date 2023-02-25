# Entity Framework Autowired Repository with Unit of Work pattern
Creating a repository interface and implementing a class is a really boring and annoying work.
When I saw JpaRepository in Java world, I wondered if there's a similar implmentation of `JpaRepository` in .NET side. I, however, couldn't find anything.
This library will liberate you from this annoying task.

# How To Use
**1. Your DbContext should have IEpaDbContext and you can define your DbSet property as usual.**
```
public ApplicationDbContext : DbContext, IEpaDbContext
{
    DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        ...
    }
}
```

**2. Add the dependency injection in the Program.cs**
```
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

// Below dependency injection is required. 
builder.Services.AddScoped<IEpaDbContext, ApplicationDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
```

**3. Define a interface inheriting IEpaRepository interface.**
```
public interface IOrderRepository : IEpaRepository<Order, Guid>
{
}
```

**4. That's it. No interface implementation. Just get the repository and use it**
```
public class IndexModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;
    public IReadOnlyCollection<Order> Orders;

    public IndexModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task OnGetAsync()
    {
        var orderRepository = _unitOfWork.GetRepository<IOrderRepository>();
        Orders = await orderRepository.GetAsync();
    }
}
```

Please see the WebApplication project for the demo.
