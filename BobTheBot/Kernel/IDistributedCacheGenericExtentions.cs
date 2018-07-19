using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading;
using Microsoft.Extensions.Caching.Distributed;

namespace BobTheBot.Kernel
{
    public static partial class IDistributedCacheGenericExtentions
    {
        public static T GetAs<T>(this IDistributedCache cache, string key)
        {
            return cache.GetAs<T>(key, Encoding.UTF8);
        }

        public static T GetAs<T>(this IDistributedCache cache, string key, Encoding encoding)
        {
            var bytes = cache.GetString(key);
            return Get<T>(bytes, encoding);
        }

        public static async Task<T> GetAsAsync<T>(
            this IDistributedCache cache,
            string key,
            CancellationToken token = default(CancellationToken))
        {
            return await cache.GetAsAsync<T>(key, Encoding.UTF8, token: token);
        }

        public static async Task<T> GetAsAsync<T>(
            this IDistributedCache cache,
            string key,
            Encoding encoding,
            CancellationToken token = default(CancellationToken))
        {
#if NS20
            var value = await cache.GetStringAsync(key, token);
#else
            var value = await cache.GetStringAsync(key);
#endif
            return Get<T>(value, encoding);
        }

        private static T Get<T>(string value, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return default(T);
            }
            var result = JsonConvert.DeserializeObject<T>(value);
            return result;
        }

        public static void SetAs<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options)
        {
            cache.SetAs<T>(key, value, Encoding.UTF8, options);
        }

        public static void SetAs<T>(this IDistributedCache cache, string key, T value, Encoding encoding, DistributedCacheEntryOptions options)
        {
            var strValue = Get<T>(value, encoding);
            cache.SetString(key, strValue, options);
        }

        public static async Task SetAsAsync<T>(
            this IDistributedCache cache,
            string key,
            T value,
            DistributedCacheEntryOptions options,
            CancellationToken token = default(CancellationToken))
        {
            await cache.SetAsAsync<T>(key, value, Encoding.UTF8, options, token: token);
        }

        public static async Task SetAsAsync<T>(
            this IDistributedCache cache,
            string key,
            T value,
            Encoding encoding,
            DistributedCacheEntryOptions options,
            CancellationToken token = default(CancellationToken))
        {
            var strValue = Get<T>(value, encoding);
#if NS20
            await cache.SetStringAsync(key, strValue, options, token);
#else
            await cache.SetStringAsync(key, strValue, options);
#endif
        }

        private static string Get<T>(T value, Encoding encoding)
        {
            if (value == null)
            {
                return "";
            }
            var serialized = JsonConvert.SerializeObject(value);
            return serialized;
        }
    }
}
