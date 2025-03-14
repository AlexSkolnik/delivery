using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Models.SharedKernel;
using Primitives;

namespace DeliveryApp.Core.Domain.Models.CourierAggregate;

public class Courier : Aggregate<Guid>
{
    // Name (string, имя курьера)
    public string Name { get; private set; }

    // Transport (Transport, транспорт курьера)
    public TransportEntity CurrentTransport { get; private set; }

    // Location (Location, местоположение курьера)
    public Location CurrentLocation { get; private set; }

    // Status (CourierStatus, статус курьера)
    public CourierStatus Status { get; private set; }

    [ExcludeFromCodeCoverage]
    private Courier()
    {
    }

    private Courier(string name, TransportEntity currentTransport, Location location) : this()
    {
        Id = Guid.NewGuid();
        Name = name;
        CurrentTransport = currentTransport;
        CurrentLocation = location;
        Status = CourierStatus.Free;
    }

    public static Result<Courier, Error> Create(string name, TransportEntity transport, Location location)
    {
        if (string.IsNullOrEmpty(name)) return GeneralErrors.ValueIsRequired(nameof(name));
        if (transport == null) return GeneralErrors.ValueIsRequired(nameof(transport));
        if (location == null) return GeneralErrors.ValueIsRequired(nameof(location));

        return new Courier(name, transport, location);
    }

    public void SetFree()
    {
        Status = CourierStatus.Free;
    }

    public UnitResult<Error> SetBusy()
    {
        if (Status == CourierStatus.Busy) return Errors.TryAssignOrderWhenCourierHasAlreadyBusy();

        Status = CourierStatus.Busy;
        return UnitResult.Success<Error>();
    }

    public Result<double, Error> CalculateTimeToLocation(Location location)
    {
        if (location == null) return GeneralErrors.ValueIsRequired(nameof(location));

        var distanceToResult = CurrentLocation.GetDistanceToLocation(location);
        if (distanceToResult.IsFailure) return distanceToResult.Error;
        var distance = distanceToResult.Value;

        var time = (double)distance / CurrentTransport.Speed;
        return time;
    }

    public UnitResult<Error> Move(Location targetLocation)
    {
        if (targetLocation == null) return GeneralErrors.ValueIsRequired(nameof(targetLocation));

        var difX = targetLocation.X - CurrentLocation.X;
        var difY = targetLocation.Y - CurrentLocation.Y;

        var newX = CurrentLocation.X;
        var newY = CurrentLocation.Y;

        var cruisingRange = CurrentTransport.Speed;

        if (difX > 0)
        {
            if (difX >= cruisingRange)
            {
                newX += cruisingRange;
                CurrentLocation = Location.Create(newX, newY).Value;
                return UnitResult.Success<Error>();
            }

            if (difX < cruisingRange)
            {
                newX += difX;
                CurrentLocation = Location.Create(newX, newY).Value;
                if (CurrentLocation == targetLocation)
                    return UnitResult.Success<Error>();
                cruisingRange -= difX;
            }
        }

        if (difX < 0)
        {
            if (Math.Abs(difX) >= cruisingRange)
            {
                newX -= cruisingRange;
                CurrentLocation = Location.Create(newX, newY).Value;
                return UnitResult.Success<Error>();
            }

            if (Math.Abs(difX) < cruisingRange)
            {
                newX -= Math.Abs(difX);
                CurrentLocation = Location.Create(newX, newY).Value;
                if (CurrentLocation == targetLocation)
                    return UnitResult.Success<Error>();
                cruisingRange -= Math.Abs(difX);
            }
        }

        if (difY > 0)
        {
            if (difY >= cruisingRange)
            {
                newY += cruisingRange;
                CurrentLocation = Location.Create(newX, newY).Value;
                return UnitResult.Success<Error>();
            }

            if (difY < cruisingRange)
            {
                newY += difY;
                CurrentLocation = Location.Create(newX, newY).Value;
                if (CurrentLocation == targetLocation)
                    return UnitResult.Success<Error>();
            }
        }

        if (difY < 0)
        {
            if (Math.Abs(difY) >= cruisingRange)
            {
                newY -= cruisingRange;
                CurrentLocation = Location.Create(newX, newY).Value;
                return UnitResult.Success<Error>();
            }

            if (Math.Abs(difY) < cruisingRange)
            {
                newY -= Math.Abs(difY);
                CurrentLocation = Location.Create(newX, newY).Value;
                if (CurrentLocation == targetLocation)
                    return UnitResult.Success<Error>();
            }
        }

        CurrentLocation = Location.Create(newX, newY).Value;
        return UnitResult.Success<Error>();
    }

    [ExcludeFromCodeCoverage]
    public static class Errors
    {
        public static Error TryAssignOrderWhenCourierHasAlreadyBusy()
        {
            return new Error($"{nameof(Courier).ToLowerInvariant()}.try.assign.order.when.courier.has.already.busy",
                "Нельзя взять заказ в работу, если курьер уже занят");
        }
    }
}