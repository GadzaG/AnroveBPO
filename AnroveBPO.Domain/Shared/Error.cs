using System.Text.Json.Serialization;

namespace AnroveBPO.Domain.Shared;

public record ErrorMessage(string Code, string Message, string? InvalidField = null);

public record Error
{
    private const string DefaultMessage = "Unknown error";

    public IReadOnlyList<ErrorMessage> Messages { get; } = [];

    public ErrorType Type { get; }

    [JsonConstructor]
    private Error(IReadOnlyList<ErrorMessage> messages, ErrorType type)
    {
        Messages = messages.ToArray();
        Type = type;
    }

    private Error(IEnumerable<ErrorMessage> messages, ErrorType type)
    {
        Messages = messages.ToArray();
        Type = type;
    }

    public string GetMessage(string separator = "\n")
    {
        if (Messages.Count == 0)
            return DefaultMessage;

        IEnumerable<string> lines = Messages
            .Select(FormatMessage)
            .Where(message => !string.IsNullOrWhiteSpace(message))
            .Distinct();

        string combined = string.Join(separator, lines);
        return string.IsNullOrWhiteSpace(combined) ? DefaultMessage : combined;
    }

    private static string FormatMessage(ErrorMessage errorMessage)
    {
        if (string.IsNullOrWhiteSpace(errorMessage.InvalidField))
            return errorMessage.Message;

        return $"{errorMessage.Message} (field: {errorMessage.InvalidField})";
    }

    public static Error Validation(string code, string message, string? invalidField = null) =>
        new([new ErrorMessage(code, message, invalidField)], ErrorType.VALIDATION);

    public static Error NotFound(string code, string message, string? invalidField = null) =>
        new([new ErrorMessage(code, message, invalidField)], ErrorType.NOT_FOUND);

    public static Error Failure(string code, string message, string? invalidField = null) =>
        new([new ErrorMessage(code, message, invalidField)], ErrorType.FAILURE);

    public static Error Conflict(string code, string message, string? invalidField = null) =>
        new([new ErrorMessage(code, message, invalidField)], ErrorType.CONFLICT);

    public static Error Authentication(string code, string message, string? invalidField = null) =>
        new([new ErrorMessage(code, message, invalidField)], ErrorType.AUTHENTICATION);

    public static Error Authorization(string code, string message, string? invalidField = null) =>
        new([new ErrorMessage(code, message, invalidField)], ErrorType.AUTHORIZATION);

    public static Error Validation(params IEnumerable<ErrorMessage> messages) =>
        new(messages, ErrorType.VALIDATION);

    public static Error NotFound(params IEnumerable<ErrorMessage> messages) =>
        new(messages, ErrorType.NOT_FOUND);

    public static Error Failure(params IEnumerable<ErrorMessage> messages) =>
        new(messages, ErrorType.FAILURE);

    public static Error Conflict(params IEnumerable<ErrorMessage> messages) =>
        new(messages, ErrorType.CONFLICT);

    public static Error Authentication(params IEnumerable<ErrorMessage> messages) =>
        new(messages, ErrorType.AUTHENTICATION);

    public static Error Authorization(params IEnumerable<ErrorMessage> messages) =>
        new(messages, ErrorType.AUTHORIZATION);
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ErrorType
{
    VALIDATION,
    NOT_FOUND,
    FAILURE,
    CONFLICT,
    AUTHENTICATION,
    AUTHORIZATION,
}