using System.Text.RegularExpressions;
using AnroveBPO.Domain.Shared;
using CSharpFunctionalExtensions;

namespace AnroveBPO.Domain.Models.ValueObjects;

public partial record ItemCode
{
    private const string FieldName = "item_code";
    private const string InvalidFormatMessage = "Код товара должен быть в формате XX-XXXX-YYXX";
    private static readonly Regex CodeRegex = MyRegex();

    public string Value { get; }
    
    private ItemCode(string value)
    {
        Value = value;
    }

    public static Result<ItemCode, Error> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return GeneralErrors.ValueIsRequired(FieldName);

        string normalizedValue = value.Trim();
        if (!CodeRegex.IsMatch(normalizedValue))
            return Error.Validation("item_code.format.invalid", InvalidFormatMessage, FieldName);

        return new ItemCode(normalizedValue);
    }

    [GeneratedRegex(@"^\d{2}-\d{4}-[A-Z]{2}\d{2}$", RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    private static partial Regex MyRegex();
}
