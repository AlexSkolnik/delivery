using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Models.CourierAggregate;
using DeliveryApp.Core.Domain.Models.OrderAggregate;
using Primitives;

namespace DeliveryApp.Core.Domain.Services;

public class DispatchService : IDispatchService
{
    public Result<Courier, Error> Dispatch(Order order, List<Courier> couriers)
    {
        if (order == null) 
            return GeneralErrors.ValueIsRequired(nameof(order));
        if (couriers == null || couriers.Count == 0) 
            return GeneralErrors.InvalidLength(nameof(couriers));
        
        var minTime = double.MaxValue;
        Courier fastestCourier = null;
        
        foreach (var courier in couriers.Where(x => x.Status == CourierStatus.Free))
        {
            var (_, isFailure, timeToLocation, error) = courier.CalculateTimeToLocation(order.Location);
            
            if (isFailure) 
                return error;

            if (!(timeToLocation < minTime)) 
                continue;
            
            minTime = timeToLocation;
            fastestCourier = courier;
        }

        // Если подходящий курьер не был найден, возвращаем ошибку
        if (fastestCourier == null) return Errors.CourierWasNotFound();

        // Если курьер найден - назначаем заказ на курьера
        var orderAssignToCourierResult = order.Assign(fastestCourier);
        
        if (orderAssignToCourierResult.IsFailure) 
            return orderAssignToCourierResult.Error;

        // Делаем курьера занятым
        var courierSetBusyResult = fastestCourier.SetBusy();
        
        if (courierSetBusyResult.IsFailure) 
            return orderAssignToCourierResult.Error;

        return fastestCourier;
    }
    
    [ExcludeFromCodeCoverage]
    public static class Errors
    {
        public static Error CourierWasNotFound()
        {
            return new Error("courier.was.not.found", "Подходящий курьер не был найден");
        }
    }
}