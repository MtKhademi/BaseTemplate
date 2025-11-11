namespace Common.Enumerations;

public enum TypeOfEducation
{
    NotRecognize = -1,
    Bisavad = 0,
    Cikle = 1,
    Diplom = 2,
    FoghDiplom = 3,
    Lisance = 4,
    FoghLisance = 5,
    Doctor = 6
}


public static class TypeOfEducationExtention
{
    public static string GetName(this TypeOfEducation education)
    {
        switch (education)
        {
            case TypeOfEducation.Bisavad:
                return "بی سواد";
            case TypeOfEducation.Cikle:
                return "سیکل";
            case TypeOfEducation.Diplom:
                return "دیپلم";
            case TypeOfEducation.FoghDiplom:
                return "فوق دیپلم";
            case TypeOfEducation.Lisance:
                return "لیسانس";
            case TypeOfEducation.FoghLisance:
                return "فوق لیسانس";
            case TypeOfEducation.Doctor:
                return "دکتر";
            default:
                return "نامشخص";
        }
    }
    public static TypeOfEducation GetEducation(this int education)
    {
        return (TypeOfEducation)education;
    }
    public static TypeOfEducation GetEducation(this string education)
    {
        if (education.Trim() == "بی سواد")
            return TypeOfEducation.Bisavad;
        if (education.Trim() == "سیکل")
            return TypeOfEducation.Bisavad;
        if (education.Trim() == "دیپلم")
            return TypeOfEducation.Bisavad;
        if (education.Trim() == "فوق دیپلم")
            return TypeOfEducation.Bisavad;
        if (education.Trim() == "لیسانس")
            return TypeOfEducation.Bisavad;
        if (education.Trim() == "فوق لیسانس")
            return TypeOfEducation.Bisavad;
        if (education.Trim() == "دکتر")
            return TypeOfEducation.Bisavad;
        return TypeOfEducation.NotRecognize;
    }

    public static TypeOfEducation ConvertToTypeOfEducation(this int? value)
    {
        if (!value.HasValue)
            return TypeOfEducation.NotRecognize;
        return (TypeOfEducation)value.Value;
    }
}
