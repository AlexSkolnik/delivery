using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Models.OrderAggregate;
using DeliveryApp.Core.Ports;

namespace DeliveryApp.Infrastructure.OutputAdapters.Postgres.Repositories;

public class OrderRepository(ApplicationDbContext context) : IOrderRepository
{
    private readonly ApplicationDbContext _context = context;

    public Task AddAsync(Order order)
    {
        throw new NotImplementedException();
    }

    public void Update(Order order)
    {
        throw new NotImplementedException();
    }

    public Task<Maybe<Order>> GetAsync(Guid orderId)
    {
        throw new NotImplementedException();
    }

    public Task<Maybe<Order>> GetFirstInCreatedStatus()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Order> GetAllAssigned()
    {
        throw new NotImplementedException();
    }
}
