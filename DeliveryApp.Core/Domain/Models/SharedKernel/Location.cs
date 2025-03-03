using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using Primitives;

namespace DeliveryApp.Core.Domain.Models.SharedKernel;

public class Location : ValueObject
{
    /// <summary>
    /// Ctr
    /// </summary>
    [ExcludeFromCodeCoverage]
    private Location()
    {
    }

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="x">X coordinate</param>
    /// <param name="y">Y coordinate</param>
    private Location(int x, int y) : this()
    {
        X = x;
        Y = y;
    }

    public int X { get; }
    public int Y { get; }

    public static Result<Location, Error> Create(int x, int y)
    {
        if (x is < 1 or > 10) return GeneralErrors.ValueIsInvalid(nameof(x));
        if (y is < 1 or > 10) return GeneralErrors.ValueIsInvalid(nameof(y));

        return new Location(x, y);
    }

    public int GetDistanceToLocation(Location start)
    {
        if (start == this)
            return 0;

        return Convert.ToInt32(Math.Abs(start.X - X) + Math.Abs(start.Y - Y));
    }

    public static Location CreateRandomLocation()
    {
        var random = new Random();

        random.Next(1, 100);

        return new Location(random.Next(1, 10), random.Next(1, 10));
    }

    /// <summary>
    /// Перегрузка для определения идентичности
    /// </summary>
    /// <returns>Результат</returns>
    /// <remarks>Идентичность будет происходить по совокупности полей указанных в методе</remarks>
    [ExcludeFromCodeCoverage]
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return X;
        yield return Y;
    }
}