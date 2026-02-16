using System.Text.Json;

namespace Mazada.Extensions;

// From Stockoverflow - Is there a way to assign a List of Items to a Session and then call it using ASP.NET CORE 6
public static class SessionExtensions 
{
    public static void SetObjectAsJson(this ISession session, string key, object value)
    {
        session.SetString(key, JsonSerializer.Serialize(value));
    }

    public static T? GetObjectFromJson<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default: JsonSerializer.Deserialize<T>(value);
    }

}