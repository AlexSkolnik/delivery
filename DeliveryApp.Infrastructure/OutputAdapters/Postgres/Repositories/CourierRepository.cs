using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Models.CourierAggregate;
using DeliveryApp.Core.Ports;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.Infrastructure.OutputAdapters.Postgres.Repositories;

public class CourierRepository(ApplicationDbContext dbContext) : ICourierRepository
{
    public async Task AddAsync(Courier courier)
    {
        await dbContext.Couriers.AddAsync(courier);
    }

    public void Update(Courier courier)
    {
        dbContext.Couriers.Update(courier);
    }

    public async Task<Maybe<Courier>> GetAsync(Guid courierId)
    {
        var courier =
            await dbContext.Couriers
                .Include(x => x.CurrentTransport)
                .FirstOrDefaultAsync(o => o.Id == courierId);

        return courier;
    }

    public async Task<IReadOnlyCollection<Courier>> GetAllFree()
    {
        var couriers = await
            dbContext.Couriers
                .Include(x => x.CurrentTransport)
                .Where(o => o.Status.Name == CourierStatus.Free.Name)
                .ToListAsync();

        return couriers;
    }
}