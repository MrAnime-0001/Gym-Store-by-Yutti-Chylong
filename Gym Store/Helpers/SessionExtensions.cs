using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Gym_Store.Helpers
{
    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            // Use Newtonsoft.Json serialization here:
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            // Use Newtonsoft.Json deserialization here:
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}