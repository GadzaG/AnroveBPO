using AnroveBPO.Domain.Shared;
using CSharpFunctionalExtensions;

namespace AnroveBPO.Domain.Models.ValueObjects;

public record Discount
{
    public double Value { get; }

    private Discount(double value)
    {
        Value = value;
    }

    public static Result<Discount, Error> Create(double value)
    {
        if (value is < 0 or > 100)
        {
            return GeneralErrors.ValueIsInvalid("Discount");
        }
        return new Discount(value);
    }
}