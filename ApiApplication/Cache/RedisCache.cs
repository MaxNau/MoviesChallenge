using ApiApplication.Serializers;
using StackExchange.Redis;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ApiApplication.Cache
{
    public class RedisCache : ICache
    {
        private readonly IDatabase _cache;
        private readonly ISerializer _serializer;

        public RedisCache(
            IConnectionMultiplexer connectionMultiplexer,
            ISerializer serializer)
        {
            _cache = connectionMultiplexer.GetDatabase();
            _serializer = serializer;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            using var ms = new MemoryStream(await _cache.StringGetAsync(key));
            return _serializer.Deserialize<T>(ms);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            using var stream = new MemoryStream();
            _serializer.Serialize(value, stream);
            await _cache.StringSetAsync(key, RedisValue.CreateFrom(stream), expiry);
        }
    }
}
