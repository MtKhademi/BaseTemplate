namespace Common.Extentions;

public static class ExcellFileExtentions
{
    public static string? GetStringValue(this ExcelWorksheet ewk, int row, int col)
    {
        if (ewk.Cells[row, col] == null || ewk.Cells[row, col].Value == null)
        {
            throw new NotValidDataException($"Cell[${row},${col}] is null");
        }
        return ewk.Cells[row, col].Value.ToString();

    }
    public static bool TryGetStringValue(this ExcelWorksheet ewk, int row, int col, out string? result)
    {
        result = default;
        if (ewk.Cells[row, col] == null || ewk.Cells[row, col].Value == null)
        {
            return false;
        }
        result = ewk.Cells[row, col].Value.ToString();
        return true;
    }

    public static int GetIntValue(this ExcelWorksheet ewk, int row, int col)
    {
        var value = ewk.GetStringValue(row, col);
        if (string.IsNullOrWhiteSpace(value))
            throw new NotValidDataException($"Cell[${row},${col}] is empty,");
        try
        {
            return int.Parse(value);
        }
        catch
        {
            throw new NotValidDataException($"Cell[${row},${col}] can not convert to int : {value}");
        }
    }
    public static double GetDoubleValue(this ExcelWorksheet ewk, int row, int col)
    {
        var value = ewk.GetStringValue(row, col);
        if (string.IsNullOrWhiteSpace(value))
            throw new NotValidDataException($"Cell[${row},${col}] is empty,");
        try
        {
            return double.Parse(value);
        }
        catch
        {
            throw new NotValidDataException($"Cell[${row},${col}] can not convert to double : {value}");
        }
    }
    public static decimal GetDecimalValue(this ExcelWorksheet ewk, int row, int col)
    {
        var value = ewk.GetStringValue(row, col);
        if (string.IsNullOrWhiteSpace(value))
            throw new NotValidDataException($"Cell[${row},${col}] is empty,");
        try
        {
            return decimal.Parse(value);
        }
        catch
        {
            throw new NotValidDataException($"Cell[${row},${col}] can not convert to double : {value}");
        }
    }
    public static bool TryGetDecimalValue(this ExcelWorksheet ewk, int row, int col, out decimal result)
    {
        result = default;
        var value = ewk.GetStringValue(row, col);
        if (string.IsNullOrWhiteSpace(value))
            return false;
        try
        {
            result = decimal.Parse(value);
            return true;
        }
        catch
        {
            return false;
        }
    }
    public static DateTime GetDateTimeValue(this ExcelWorksheet ewk, int row, int col, Func<string, DateTime> convertFunction)
    {
        var value = ewk.GetStringValue(row, col);
        if (string.IsNullOrWhiteSpace(value))
            throw new NotValidDataException($"Cell[${row},${col}] is empty,");

        return convertFunction(value);
    }


}
