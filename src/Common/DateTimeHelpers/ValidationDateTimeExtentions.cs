namespace Common.DateTimeHelpers;

public static class ValidationDateTimeExtensions
{
    public static bool ValidateDateIfExists(this string? value, IDateTimeFormatter formatter, out string message)
    {
        message = $"Cannot convert to date. Expected format: YYYY-MM-DD :: {value}";
        if (string.IsNullOrWhiteSpace(value))
            return true;
        return formatter.IsValidDate(value);
    }

    public static bool ValidateDateTimeIfExists(this string? value, IDateTimeFormatter formatter, out string message)
    {
        message = $"Cannot convert to date-time. Expected format: YYYY-MM-DDThh:mm:ss :: {value}";
        if (string.IsNullOrWhiteSpace(value))
            return true;
        return formatter.IsValidDateTime(value);
    }

    public static bool ValidateDate(this string? value, IDateTimeFormatter formatter, List<string> errors, string propertyName = "")
    {
        if (formatter.IsValidDate(value ?? string.Empty))
            return true;

        errors.Add($"{propertyName} cannot be converted to date. Expected format: YYYY-MM-DD :: {value}");
        return false;
    }

    public static bool ValidateDate(this string? value, IDateTimeFormatter formatter, out string message)
    {
        message = $"Cannot convert to date. Expected format: YYYY-MM-DD :: {value}";
        if (string.IsNullOrWhiteSpace(value))
            return false;

        return formatter.IsValidDate(value);
    }

    public static bool ValidateDateTime(this string? value, IDateTimeFormatter formatter, out string message)
    {
        message = $"Cannot convert to date-time. Expected format: YYYY-MM-DDThh:mm:ss :: {value}";
        if (string.IsNullOrWhiteSpace(value))
            return true;
        return formatter.IsValidDateTime(value);
    }
}