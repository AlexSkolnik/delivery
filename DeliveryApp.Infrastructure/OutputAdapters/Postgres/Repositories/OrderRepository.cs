using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Models.OrderAggregate;
using DeliveryApp.Core.Ports;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.Infrastructure.OutputAdapters.Postgres.Repositories;

public class OrderRepository(ApplicationDbContext dbContext) : IOrderRepository
{
    public async Task AddAsync(Order order)
    {
        await dbContext.Orders.AddAsync(order);
    }

    public void Update(Order order)
    {
        dbContext.Orders.Update(order);
    }

    public async Task<Maybe<Order>> GetAsync(Guid orderId)
    {
        var order = await dbContext.Orders
            .SingleOrDefaultAsync(o => o.Id == orderId);

        return order;
    }

    public async Task<Maybe<Order>> GetFirstInCreatedStatus()
    {
        var order =
            await dbContext.Orders
                .FirstOrDefaultAsync(o => o.Status.Name == OrderStatus.Created.Name);

        return order;
    }

    public IEnumerable<Order> GetAllAssigned()
    {
        var orders =
            dbContext.Orders
                .Where(o => o.Status.Name == OrderStatus.Assigned.Name);
        
        return orders;
    }
}