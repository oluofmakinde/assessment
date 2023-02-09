using Assessment.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Assessment.Caching
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void CacheResponse(string key, object data, TimeSpan cacheTime)
        {
            if (data == null || string.IsNullOrEmpty(key))
                return;

            var serializedData = JsonConvert.SerializeObject(data);

             _cache.Set(key, serializedData,cacheTime);
        }


        public string GetCacheResponse(string key)
        {
            return _cache.Get<string>(key);
        }

        public bool IsSet(string key)
        {
            var data = _cache.Get<string>(key);

            return data != null;
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}
