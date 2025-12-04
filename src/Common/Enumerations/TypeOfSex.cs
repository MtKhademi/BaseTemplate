using Infrastructure.Extentions;

namespace Infrastructure.Enumerations;

public enum TypeOfSex
{
    NotRecognize = -1,
    Men = 0,
    Women = 1,
}

public static class TypeOfSexExtentions
{
    public static TypeOfSex ConvertToTypeOfSex(this int? value)
    {
        if (!value.HasValue)
            return TypeOfSex.NotRecognize;
        return (TypeOfSex)value.Value;
    }
}