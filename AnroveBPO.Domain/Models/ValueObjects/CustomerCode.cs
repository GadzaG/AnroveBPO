using System.Text.RegularExpressions;
using AnroveBPO.Domain.Shared;
using CSharpFunctionalExtensions;

namespace AnroveBPO.Domain.Models.ValueObjects;

public record CustomerCode
{
    private const string InvalidFormatMessage = "Код заказчика должен быть в формате XXXX-ГГГГ";
    private const string FieldName = "customer_code";
    private const int MinRegistrationYear = 1900;
    private static readonly Regex CodeRegex = new(
        @"^(?<number>\d{4})-(?<year>\d{4})$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public string Value { get; }

    public int RegistrationYear { get; }

    private CustomerCode(string value, int registrationYear)
    {
        Value = value;
        RegistrationYear = registrationYear;
    }

    public static Result<CustomerCode, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return GeneralErrors.ValueIsRequired(FieldName);

        string normalizedValue = value.Trim();
        Match match = CodeRegex.Match(normalizedValue);
        if (!match.Success)
            return Error.Validation("customer_code.format.invalid", InvalidFormatMessage, FieldName);

        if (!int.TryParse(match.Groups["year"].Value, out int registrationYear))
            return Error.Validation("customer_code.year.invalid", "Год регистрации заказчика некорректен", FieldName);

        int currentYear = DateTime.UtcNow.Year;
        if (registrationYear < MinRegistrationYear || registrationYear > currentYear)
        {
            return Error.Validation(
                "customer_code.year.out_of_range",
                $"Год регистрации заказчика должен быть в диапазоне {MinRegistrationYear}-{currentYear}",
                FieldName);
        }

        return new CustomerCode(normalizedValue, registrationYear);
    }
}
