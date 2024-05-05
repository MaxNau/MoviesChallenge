using System.Threading.Tasks;
using System;

namespace ApiApplication.Cache
{
    public interface ICache
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
    }
}
