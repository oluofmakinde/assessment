namespace Assessment.Interfaces
{
    public interface ICacheService
    {
        string GetCacheResponse(string key);
        void CacheResponse(string key, object response, TimeSpan cacheTime);
        bool IsSet(string key);
        void Remove(string key);
    }
}
