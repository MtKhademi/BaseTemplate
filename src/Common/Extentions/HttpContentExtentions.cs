using System.Text;

namespace Common.Extentions;

public static class HttpContentExtentions
{
    public static StringContent ToContentHttp(this object o)
           => new StringContent(JsonConvert.SerializeObject(o), Encoding.UTF8, "application/json");

    public static async Task<T?> ReadModelFromJsonAsync<T>(this HttpContent content)
    {
        //var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        //options.Converters.Add(new JsonStringEnumConverter());
        //return await content.ReadFromJsonAsync<T>(options);

        var data = await content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(data);
    }

}
