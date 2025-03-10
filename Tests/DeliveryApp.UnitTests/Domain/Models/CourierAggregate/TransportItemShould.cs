using System.Collections.Generic;
using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Models.CourierAggregate;
using FluentAssertions;
using Xunit;

namespace DeliveryApp.UnitTests.Domain.Models.CourierAggregate;

public sealed class TransportItemShould : Entity<int>
{
    public static IEnumerable<object[]> GetTransports()
    {
        yield return [TransportItem.Pedestrian, 1, "pedestrian", 1];
        yield return [TransportItem.Bicycle, 2, "bicycle", 2];
        yield return [TransportItem.Car, 3, "car", 3];
    }

    [Theory]
    [MemberData(nameof(GetTransports))]
    public void ReturnCorrectIdAndName(TransportItem transportItem, int id, string name, int speed)
    {
        //Arrange

        //Act

        //Assert
        transportItem.Id.Should().Be(id);
        transportItem.Name.Should().Be(name);
        transportItem.Speed.Should().Be(speed);
    }

    [Theory]
    [MemberData(nameof(GetTransports))]
    public void CanBeFoundById(TransportItem transportItem, int id, string name, int speed)
    {
        //Arrange

        //Act
        var result = TransportItem.GetById(id).Value;

        //Assert
        result.Should().Be(transportItem);
        result.Id.Should().Be(id);
        result.Name.Should().Be(name);
        result.Speed.Should().Be(speed);
    }

    [Theory]
    [MemberData(nameof(GetTransports))]
    public void CanBeFoundByName(TransportItem transportItem, int id, string name, int speed)
    {
        //Arrange

        //Act
        var result = TransportItem.GetByName(name).Value;

        //Assert
        result.Should().Be(transportItem);
        result.Id.Should().Be(id);
        result.Name.Should().Be(name);
        result.Speed.Should().Be(speed);
    }

    [Fact]
    public void ReturnErrorWhenTransportNotFoundById()
    {
        //Arrange
        var id = -1;

        //Act
        var result = TransportItem.GetById(id);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
    }

    [Fact]
    public void ReturnErrorWhenTransportNotFoundByName()
    {
        //Arrange
        var name = "not-existed-TransportItem";

        //Act
        var result = TransportItem.GetByName(name);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
    }

    [Fact]
    public void ReturnListOfStatuses()
    {
        //Arrange

        //Act
        var allStatuses = TransportItem.GetItems();

        //Assert
        allStatuses.Should().NotBeEmpty();
    }

    [Fact]
    public void DerivedEntity()
    {
        //Arrange

        //Act
        var isDerivedEntity = typeof(TransportItem).IsSubclassOf(typeof(Entity<int>));

        //Assert
        isDerivedEntity.Should().BeTrue();
    }

    [Fact]
    public void BeEqualWhenIdIsEqual()
    {
        //Arrange
        var pedestrian1 = TransportItem.Pedestrian;
        var pedestrian2 = TransportItem.Pedestrian;
        pedestrian1.Id.Should().Be(pedestrian2.Id);

        //Act
        var result = pedestrian1.Equals(pedestrian2);

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void NotBeEqualWhenIdIsNotEqual()
    {
        //Arrange
        var pedestrian = TransportItem.Pedestrian;
        var car = TransportItem.Car;
        pedestrian.Id.Should().NotBe(car.Id);

        //Act
        var result = pedestrian.Equals(car);

        //Assert
        result.Should().BeFalse();
    }
}