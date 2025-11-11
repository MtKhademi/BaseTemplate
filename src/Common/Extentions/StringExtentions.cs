namespace Common.Extentions;

public static class StringExtentions
{
    public const string SEPERATOR = "|";

    public static string InLine(this IEnumerable<string> value, string seperator = SEPERATOR)
    {
        return string.Join(seperator, value);
    }
    public static string ToCamaleCase(this string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return value;
        return char.ToLower(value[0]) + value.Substring(1);
    }


    public static string ToPersianChar(this string value)
    {
        return value
            .Replace("ﮎ", "ک")
                  .Replace("ﮏ", "ک")
                  .Replace("ﮐ", "ک")
                  .Replace("ﮑ", "ک")
                  .Replace("ك", "ک")
                  .Replace("ي", "ی")
                  .Replace("ئ", "ی")
                  .Replace("ى", "ی")
                  //.Replace(" ", " ")
                  //.Replace("‌", " ")
                  //.Replace("ٔ", "")
                  .Replace("ھ", "ه")
                  .Replace("دِ", "د")
                  .Replace("بِ", "ب")
                  .Replace("زِ", "ز")
                  .Replace("شِ", "ش")
                  .Replace("سِ", "س");
    }

    public static string ToEnglishNumber(this string value)
    {
        //۰/۰/
        return value
            .Replace("۰", "0")
            .Replace("۱", "1")
            .Replace("۲", "2")
            .Replace("۳", "3")
            .Replace("۴", "4")
            .Replace("۵", "5")
            .Replace("۶", "6")
            .Replace("۷", "7")
            .Replace("۸", "8")
            .Replace("۹", "9");
    }



    public static string TryGetFirstValueIsNullOrWhiteSpace(this string? value, params string?[]? otherValues)
    {
        if (!string.IsNullOrWhiteSpace(value))
            return value;

        if (otherValues == null || otherValues.Length == 0)
            return "";

        foreach (var item in otherValues)
        {
            if (!string.IsNullOrWhiteSpace(item))
                return item;
        }

        return "";
    }

    public static string FixPersianChars(this string str)
    {
        return str.Replace("ﮎ", "ک")
                  .Replace("ﮏ", "ک")
                  .Replace("ﮐ", "ک")
                  .Replace("ﮑ", "ک")
                  .Replace("ك", "ک")
                  .Replace("ي", "ی")
                  .Replace("ئ", "ی")
                  .Replace("ى", "ی")
                  .Replace(" ", " ")
                  .Replace("‌", " ")
                  .Replace("ٔ", "")
                  .Replace("ھ", "ه")
                  .Replace("دِ", "د")
                  .Replace("بِ", "ب")
                  .Replace("زِ", "ز")
                  .Replace("شِ", "ش")
                  .Replace("سِ", "س");
    }
}
