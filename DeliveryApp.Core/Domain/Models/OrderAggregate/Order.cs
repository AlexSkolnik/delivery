﻿using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Models.CourierAggregate;
using DeliveryApp.Core.Domain.Models.SharedKernel;
using Primitives;

namespace DeliveryApp.Core.Domain.Models.OrderAggregate;

/// <summary>
///     Заказ
/// </summary>
public class Order : Aggregate<Guid>
{
    /// <summary>
    ///     Ctr
    /// </summary>
    [ExcludeFromCodeCoverage]
    private Order()
    {
    }

    /// <summary>
    ///     Ctr
    /// </summary>
    /// <param name="orderId">Идентификатор заказа</param>
    /// <param name="location">Геопозиция</param>
    private Order(Guid orderId, Location location) : base(orderId)
    {
        Location = location;
        Status = OrderStatus.Created;
    }

    /// <summary>
    ///     Идентификатор исполнителя (курьера)
    /// </summary>
    public Guid? CourierId { get; private set; }

    /// <summary>
    ///     Местоположение, куда нужно доставить заказ
    /// </summary>
    public Location Location { get; private set; }

    /// <summary>
    ///     Статус
    /// </summary>
    public OrderStatus Status { get; private set; }

    /// <summary>
    ///     Factory Method
    /// </summary>
    /// <param name="orderId">Идентификатор заказа</param>
    /// <param name="location">Геопозиция</param>
    /// <returns>Результат</returns>
    public static Result<Order, Error> Create(Guid orderId, Location location)
    {
        if (orderId == Guid.Empty)
            return GeneralErrors.ValueIsRequired(nameof(orderId));

        if (location == null)
            return GeneralErrors.ValueIsRequired(nameof(location));

        return new Order(orderId, location);
    }

    /// <summary>
    ///     Назначить заказ на курьера
    /// </summary>
    /// <param name="courier">Курьер</param>
    /// <returns>Результат</returns>
    public UnitResult<Error> Assign(Courier courier)
    {
        if (courier == null)
            return GeneralErrors.ValueIsRequired(nameof(courier));

        if (courier.Status == CourierStatus.Busy)
            return Errors.CantAssignOrderToBusyCourier(courier.Id);

        if (Status == OrderStatus.Completed)
            return Errors.CantAssignOrderWhenOrderStatusCompleted();

        if (Status == OrderStatus.Assigned)
            return Errors.CantAssignOrderWhenOrderStatusAssigned();

        CourierId = courier.Id;
        Status = OrderStatus.Assigned;

        return UnitResult.Success<Error>();
    }

    /// <summary>
    ///     Завершить выполнение заказа
    /// </summary>
    /// <returns>Результат</returns>
    public UnitResult<Error> Complete()
    {
        if (Status != OrderStatus.Assigned)
            return Errors.CantCompletedNotAssignedOrder();

        Status = OrderStatus.Completed;

        return UnitResult.Success<Error>();
    }

    /// <summary>
    ///     Ошибки, которые может возвращать сущность
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class Errors
    {
        public static Error CantCompletedNotAssignedOrder()
        {
            return new Error($"{nameof(Order).ToLowerInvariant()}.cant.completed.not.assigned.order",
                "Нельзя завершить заказ, который не был назначен");
        }

        public static Error CantAssignOrderToBusyCourier(Guid courierId)
        {
            return new Error($"{nameof(Order).ToLowerInvariant()}.cant.assign.order.to.busy.courier",
                $"Нельзя назначить заказ на курьера, который занят. Id курьера = {courierId}");
        }

        public static Error CantAssignOrderWhenOrderStatusCompleted()
        {
            return new Error($"{nameof(Order).ToLowerInvariant()}.cant.assign.order.to.busy.courier",
                $"Нельзя назначить заказ на курьера, который уже завершен.");
        }

        public static Error CantAssignOrderWhenOrderStatusAssigned()
        {
            return new Error($"{nameof(Order).ToLowerInvariant()}.cant.assign.order.to.busy.courier",
                $"Нельзя назначить заказ на курьера, который уже назначен.");
        }
    }
}