using Castle.DynamicProxy;

namespace Microsoft.EntityFrameworkCore.PersistenceApi;

/// <summary>
/// An interceptor class to create an instance for any repository interface inheriting <see cref="IEpaRepository{TEntity,TKey}"/>
/// </summary>
internal class EpaRepositoryInterceptor : IInterceptor
{
    private readonly IEpaDbContext _dbContext;

    public EpaRepositoryInterceptor(IEpaDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public void Intercept(IInvocation invocation)
    {
        var args = invocation.Method.DeclaringType?.GenericTypeArguments;
        if (args is not { Length: 2 })
        {
            throw new InvalidOperationException("The interface should have two generic types, Entity and Key.");
        }

        var type = typeof(EpaRepository<,>).MakeGenericType(args[0], args[1]);

        var instance = Activator.CreateInstance(type, _dbContext);
        var method = instance?.GetType().GetMethod(invocation.Method.Name);
        var resultSet = method?.Invoke(instance, invocation.Arguments);

        invocation.ReturnValue = resultSet;
    }
}