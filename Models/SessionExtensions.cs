using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace WebShopMVC.Models
{
    public static class SessionExtensions
    {
        //Kompleks nesneleri sessionda tutamıyoruz, buna çözüm olarak sessionda string değer taşıyabiliyoruz
        //Kompleks nesneyi json formatına çevirip bu string veriyi sessiona yazıyoruz.
        
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T? Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
