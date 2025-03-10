using System.Collections.Generic;
using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Models.CourierAggregate;
using FluentAssertions;
using Xunit;

namespace DeliveryApp.UnitTests.Domain.Models.CourierAggregate;

public class TransportEntityShould
{
    public static IEnumerable<object[]> GetTransports()
    {
        yield return [TransportEntity.Pedestrian, 1, "pedestrian", 1];
        yield return [TransportEntity.Bicycle, 2, "bicycle", 2];
        yield return [TransportEntity.Car, 3, "car", 3];
    }

    [Theory]
    [MemberData(nameof(GetTransports))]
    public void ReturnCorrectIdAndName(TransportEntity transportEntity, int id, string name, int speed)
    {
        //Arrange

        //Act

        //Assert
        transportEntity.Id.Should().Be(id);
        transportEntity.Name.Should().Be(name);
        transportEntity.Speed.Should().Be(speed);
    }

    [Theory]
    [MemberData(nameof(GetTransports))]
    public void CanBeFoundById(TransportEntity transportEntity, int id, string name, int speed)
    {
        //Arrange

        //Act
        var result = TransportEntity.GetById(id).Value;

        //Assert
        result.Should().Be(transportEntity);
        result.Id.Should().Be(id);
        result.Name.Should().Be(name);
        result.Speed.Should().Be(speed);
    }

    [Theory]
    [MemberData(nameof(GetTransports))]
    public void CanBeFoundByName(TransportEntity transportEntity, int id, string name, int speed)
    {
        //Arrange

        //Act
        var result = TransportEntity.GetByName(name).Value;

        //Assert
        result.Should().Be(transportEntity);
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
        var result = TransportEntity.GetById(id);

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
        var result = TransportEntity.GetByName(name);

        //Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
    }

    [Fact]
    public void ReturnListOfStatuses()
    {
        //Arrange

        //Act
        var allStatuses = TransportEntity.GetItems();

        //Assert
        allStatuses.Should().NotBeEmpty();
    }

    [Fact]
    public void DerivedEntity()
    {
        //Arrange

        //Act
        var isDerivedEntity = typeof(TransportEntity).IsSubclassOf(typeof(Entity<int>));

        //Assert
        isDerivedEntity.Should().BeTrue();
    }

    [Fact]
    public void BeEqualWhenIdIsEqual()
    {
        //Arrange
        var pedestrian1 = TransportEntity.Pedestrian;
        var pedestrian2 = TransportEntity.Pedestrian;
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
        var pedestrian = TransportEntity.Pedestrian;
        var car = TransportEntity.Car;
        pedestrian.Id.Should().NotBe(car.Id);

        //Act
        var result = pedestrian.Equals(car);

        //Assert
        result.Should().BeFalse();
    }
}