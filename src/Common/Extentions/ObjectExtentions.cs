using System.Text;

namespace Common.Extentions;

public static class ObjectExtentions
{
    /// <summary>
    /// convert to json string
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="o"></param>
    /// <returns></returns>
    public static string ToJson<T>(this T o) => JsonConvert.SerializeObject(o);

    /// <summary>
    /// convert from json string to model
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static T? ToModel<T>(this string value) => JsonConvert.DeserializeObject<T>(value);

    public static string ToQueryString<T>(this T obj)
    {
        string jsonString = JsonConvert.SerializeObject(obj);
        //var jsonObject = JsonConvert.DeserializeObject<JObject>(jsonString);//JObject.Parse(jsonString);


        JObject jsonObject;
        using (var reader = new JsonTextReader(new StringReader(jsonString)) { DateParseHandling = DateParseHandling.None })
            jsonObject = JObject.Load(reader);

        var properties = jsonObject
            .Properties()
            .Where(p => p.Value.Type != JTokenType.Null)
            .Select(p =>
                {

                    switch (p.Value.Type)
                    {
                        case JTokenType.Array:
                            StringBuilder result = new();
                            for (int i = 0; i < p.Value.Count(); i++)
                            {
                                result.Append($"{HttpUtility.UrlEncode(p.Name)}={HttpUtility.UrlEncode(p.Value[i].ToString())}" +
                                    $"{((i < p.Value.Count() - 1) ? "&" : "")}");
                            }
                            return result.ToString();
                        case JTokenType.None:
                        case JTokenType.Object:
                        case JTokenType.Constructor:
                        case JTokenType.Property:
                        case JTokenType.Comment:
                        case JTokenType.Integer:
                        case JTokenType.Float:
                        case JTokenType.String:
                        case JTokenType.Boolean:
                        case JTokenType.Null:
                        case JTokenType.Undefined:
                        case JTokenType.Date:
                        case JTokenType.Raw:
                        case JTokenType.Bytes:
                        case JTokenType.Guid:
                        case JTokenType.Uri:
                        case JTokenType.TimeSpan:
                        default:
                            return $"{HttpUtility.UrlEncode(p.Name)}={HttpUtility.UrlEncode(p.Value.ToString())}";
                    }
                });
        return string.Join("&", properties);
    }

}
