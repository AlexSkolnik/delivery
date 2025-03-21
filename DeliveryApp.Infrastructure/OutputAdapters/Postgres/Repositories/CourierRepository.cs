using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Models.CourierAggregate;
using DeliveryApp.Core.Ports;

namespace DeliveryApp.Infrastructure.OutputAdapters.Postgres.Repositories;

public class CourierRepository(ApplicationDbContext context) : ICourierRepository
{
    private readonly ApplicationDbContext _context = context;

    public Task AddAsync(Courier courier)
    {
        throw new NotImplementedException();
    }

    public void Update(Courier courier)
    {
        throw new NotImplementedException();
    }

    public Task<Maybe<Courier>> GetAsync(Guid courierId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Courier> GetAllFree()
    {
        throw new NotImplementedException();
    }
}