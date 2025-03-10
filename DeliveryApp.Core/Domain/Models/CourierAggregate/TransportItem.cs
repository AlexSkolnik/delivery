using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using Primitives;

namespace DeliveryApp.Core.Domain.Models.CourierAggregate;

public class TransportItem : Entity<int>
{
    public static readonly TransportItem Pedestrian = new(1, nameof(Pedestrian).ToLowerInvariant(), 1);
    public static readonly TransportItem Bicycle = new(2, nameof(Bicycle).ToLowerInvariant(), 2);
    public static readonly TransportItem Car = new(3, nameof(Car).ToLowerInvariant(), 3);

    [ExcludeFromCodeCoverage]
    private TransportItem()
    {
    }

    private TransportItem(int id, string name, int speed) : this()
    {
        Id = id;
        Name = name;
        Speed = speed;
    }

    public string Name { get; private set; }
    public int Speed { get; private set; }

    /// <summary>
    /// Список всех значений списка
    /// </summary>
    public static IEnumerable<TransportItem> GetItems()
    {
        yield return Pedestrian;
        yield return Bicycle;
        yield return Car;
    }

    /// <summary>
    /// Получить транспорт по названию
    /// </summary>
    public static Result<TransportItem, Error> GetByName(string name)
    {
        var items = GetItems()
            .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));
        
        if (items == null)
        {
            return Errors.StatusIsWrong();
        }

        return items;
    }

    /// <summary>
    /// Получить транспорт по идентификатору
    /// </summary>
    public static Result<TransportItem, Error> GetById(int id)
    {
        var state = GetItems().SingleOrDefault(s => s.Id == id);
        
        if (state == null)
        {
            return Errors.StatusIsWrong();
        }

        return state;
    }

    /// <summary>
    /// Ошибки, которые может возвращать сущность
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class Errors
    {
        public static Error StatusIsWrong()
        {
            return new Error($"{nameof(TransportItem).ToLowerInvariant()}_status_is_wrong",
                $"Не верное значение. Допустимые значения: {nameof(TransportItem).ToLowerInvariant()}: {string.Join(",", GetItems().Select(s => s.Name))}");
        }
    }
}