using Xunit;
using DeliveryApp.Core.Domain.Models.SharedKernel;
using FluentAssertions;

namespace DeliveryApp.UnitTests.Domain.Models.SharedKernel;

public class LocationShould
{
    [Fact]
    public void BeCorrectWhenParamsIsCorrectOnCreated()
    {
        //Arrange

        //Act
        var location = Location.Create(1, 2);

        //Assert
        location.IsSuccess.Should().BeTrue();
        location.Value.X.Should().Be(1);
        location.Value.Y.Should().Be(2);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    [InlineData(1, 11)]
    [InlineData(11, 1)]
    [InlineData(11, 11)]
    public void ReturnErrorWhenParamsIsInCorrectOnCreated(int x, int y)
    {
        //Arrange

        //Act
        var location = Location.Create(x, y);

        //Assert
        location.IsSuccess.Should().BeFalse();
        location.Error.Should().NotBeNull();
    }

    [Fact]
    public void BeEqualWhenAllPropertiesIsEqual()
    {
        //Arrange
        var first = Location.Create(1, 1).Value;
        var second = Location.Create(1, 1).Value;

        //Act
        var result = first == second;

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void BeNotEqualWhenNotAllPropertiesIsEqual()
    {
        //Arrange
        var first = Location.Create(1, 1).Value;
        var second = Location.Create(1, 2).Value;

        //Act
        var result = first == second;

        //Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GetDistanceToLocation()
    {
        //Arrange
        var first = Location.Create(10, 10).Value;
        var second = Location.Create(1, 1).Value;

        //Act
        var distance = first.GetDistanceToLocation(second);

        //Assert
        distance.IsSuccess.Should().BeTrue();
        distance.Value.Should().Be(18);
    }
    
    [Fact]
    public void CanCreateRandomLocation()
    {
        //Arrange

        //Act
        var location = Location.CreateRandomLocation();

        //Assert
        location.Should().NotBeNull();
        location.X.Should().BeGreaterThanOrEqualTo(1).And.BeLessThanOrEqualTo(10);
        location.Y.Should().BeGreaterThanOrEqualTo(1).And.BeLessThanOrEqualTo(10);
    }

}