﻿using System;
using System.Collections.Generic;
using DeliveryApp.Core.Domain.Models.CourierAggregate;
using DeliveryApp.Core.Domain.Models.OrderAggregate;
using DeliveryApp.Core.Domain.Models.SharedKernel;
using DeliveryApp.Core.Domain.Services;
using FluentAssertions;
using Xunit;

namespace DeliveryApp.UnitTests.Domain.Services;

public class DispatchServiceShould
{
     [Fact]
    public void FindNearestCourierForOrder()
    {
        // Arrange
        var courier1 = Courier.Create("Ваня", TransportEntity.Pedestrian, Location.Create(1, 1).Value).Value;
        var courier2 = Courier.Create("Петя", TransportEntity.Pedestrian, Location.Create(2, 2).Value).Value;
        var courier3 = Courier.Create("Маша", TransportEntity.Pedestrian, Location.Create(3, 3).Value).Value;
        List<Courier> couriers = [courier1, courier2, courier3];

        var order = Order.Create(Guid.NewGuid(), Location.Create(2, 2).Value).Value;

        // Act
        var dispatchService = new DispatchService();
        var result = dispatchService.Dispatch(order, couriers);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(courier2);
    }

    [Fact]
    public void FindFastestCourier()
    {
        // Arrange
        var courier1 = Courier.Create("Ваня", TransportEntity.Pedestrian, Location.Create(1, 1).Value).Value;
        var courier2 = Courier.Create("Петя", TransportEntity.Pedestrian, Location.Create(5, 5).Value).Value;
        var courier3 = Courier.Create("Маша", TransportEntity.Car, Location.Create(3, 3).Value).Value;
        List<Courier> couriers = [courier1, courier2, courier3];

        var order = Order.Create(Guid.NewGuid(), Location.Create(4, 4).Value).Value;

        // Act
        var dispatchService = new DispatchService();
        var result = dispatchService.Dispatch(order, couriers);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(courier3);
    }

    [Fact]
    public void ReturnValueIsRequiredErrorWhenCouriersListIsEmpty()
    {
        // Arrange
        var couriers = new List<Courier>();
        var order = Order.Create(Guid.NewGuid(), Location.Create(2, 2).Value).Value;

        // Act
        var dispatchService = new DispatchService();
        var result = dispatchService.Dispatch(order, couriers);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
    }

    [Fact]
    public void ReturnValueIsRequiredErrorWhenOrderIsNull()
    {
        // Arrange
        var courier1 = Courier.Create("Ваня", TransportEntity.Car, Location.Create(1, 1).Value).Value;
        var courier2 = Courier.Create("Петя", TransportEntity.Pedestrian, Location.Create(2, 2).Value).Value;
        var courier3 = Courier.Create("Маша", TransportEntity.Car, Location.Create(5, 5).Value).Value;
        List<Courier> couriers = [courier1, courier2, courier3];

        // Act
        var dispatchService = new DispatchService();
        var result = dispatchService.Dispatch(null, couriers);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
    }

}