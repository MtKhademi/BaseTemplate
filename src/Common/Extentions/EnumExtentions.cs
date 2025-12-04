namespace Infrastructure.Extentions;

public static class EnumExtensions
{

    public static string GetDescription(this Enum genericEnum)
    {
        Type genericEnumType = genericEnum.GetType();
        MemberInfo[] memberInfo = genericEnumType.GetMember(genericEnum.ToString());
        if ((memberInfo != null && memberInfo.Length > 0))
        {
            var _Attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            if ((_Attribs != null && _Attribs.Count() > 0))
            {
                return ((System.ComponentModel.DescriptionAttribute)_Attribs.ElementAt(0)).Description;
            }
        }
        return genericEnum.ToString();
    }
    public static IEnumerable<T> GetEnumerable<T>() where T : struct
    {
        if (!typeof(T).IsEnum)
            throw new NotSupportedException();

        return Enum.GetValues(typeof(T))
            .Cast<T>()
            .ToList();
    }
    
    public static Dictionary<int, string> ToDictionaryWithKey<T>() where T : struct
    {
        return Enum.GetValues(typeof(T)).Cast<T>()
            .ToDictionary(p => Convert.ToInt32(p), q => q.ToString());
    }
    public static Dictionary<T, string> ToDictionaryWithTypeAndDescription<T>() where T : System.Enum
    {
        return Enum.GetValues(typeof(T)).Cast<T>()
            .ToDictionary(p => p, q => GetDescription(q));
    }
    public static Dictionary<string, int> ToDictionaryWithName<T>() where T : struct
    {
        return Enum.GetValues(typeof(T)).Cast<T>()
            .ToDictionary(q => q.ToString().ToLower(), p => Convert.ToInt32(p));
    }
    public static Dictionary<string, T> ToDictionaryWithNameAndType<T>() where T : struct
    {
        return Enum.GetValues(typeof(T)).Cast<T>()
            .ToDictionary(q => q.ToString().ToLower(), p => p);
    }

    public static string GetDisplayName<T>(this T typeOfAnnouncement) where T : struct
    {
        return typeOfAnnouncement.GetType()
            .GetMember(typeOfAnnouncement.ToString())
            .First()
            .GetCustomAttribute<DisplayAttribute>() is DisplayAttribute attribute
            ? attribute.Name : "";

    }

    public static T GetRandom<T>() where T : struct
    {
        var oRandom = new Random();
        var data = GetEnumerable<T>().ToList();
        var index = oRandom.Next(0, data.Count());
        return data[index];
    }
}
