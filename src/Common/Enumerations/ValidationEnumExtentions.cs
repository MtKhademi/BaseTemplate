namespace Common.Enumerations;

public static class ValidationEnumExtensions
{
    public static bool ValidateEnumValueIfExists<T>(this string? value, ICollection<string> errors)
        where T : Enum
    {
        if (!value.ValidateEnumValueIfExists<T>(out var errorMessage))
        {
            if (!string.IsNullOrEmpty(errorMessage))
                errors.Add(errorMessage);
            return false;
        }
        return true;
    }

    public static bool ValidateEnumValue<T>(this string? value, ICollection<string> errors)
        where T : Enum
    {
        if (!value.ValidateEnumValue<T>(out var errorMessage))
        {
            errors.Add(errorMessage);
            return false;
        }
        return true;
    }

    public static bool ValidateEnumValue<T>(this string? value, out string errorMessage)
        where T : Enum
    {
        var allowedValues = string.Join(", ", Enum.GetNames(typeof(T)));
        errorMessage = $"Value for enum type '{typeof(T).Name}' must not be null or empty. Allowed values: {allowedValues}";

        if (string.IsNullOrWhiteSpace(value))
            return false;

        if (Enum.TryParse(typeof(T), value, out _))
            return true;

        return false;
    }

    public static bool ValidateEnumValueIfExists<T>(this string? value, out string errorMessage)
        where T : Enum
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            errorMessage = string.Empty;
            return true;
        }

        return value.ValidateEnumValue<T>(out errorMessage);
    }
}