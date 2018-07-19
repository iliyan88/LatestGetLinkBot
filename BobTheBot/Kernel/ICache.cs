using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobTheBot.Kernel
{
    public interface ICache<TCache, TStore>
    {
        /// <summary>
        /// Get from cache and if there is no cache, data is requested from the store then cached and returned
        /// </summary>
        /// <returns>Returns the cached data</returns>
        Task<TCache> GetAsync();

        /// <summary>
        /// Get directly from cache
        /// </summary>
        /// <returns>Returns the cache data or null if there is no cache</returns>
        Task<TCache> GetFromCacheAsync();

        /// <summary>
        /// Get from store (no cache)
        /// </summary>
        /// <returns>Returns data from store</returns>
        Task<TStore> GetFromStoreAsync();

        /// <summary>
        /// Refreshes data in the cache based on its key, resetting its sliding expiration timeout (if any).
        /// </summary>
        /// <returns></returns>
        Task RefreshAsync();

        /// <summary>
        /// Removes a cache entry based on its key.
        /// </summary>
        /// <returns></returns>
        Task RemoveAsync();

        /// <summary>
        /// Data is requested from the store then cached
        /// </summary>
        /// <param name="cacheOptions"></param>
        /// <returns>Returns the cached data</returns>
        Task<TCache> SetAsync(DistributedCacheEntryOptions cacheOptions = null);

        /// <summary>
        /// If data exist it is overridden with the most recent data from the store otherwise no action is done
        /// </summary>
        /// <param name="cacheOptions"></param>
        /// <returns></returns>
        Task InvalidateAsync(DistributedCacheEntryOptions cacheOptions = null);
    }
}
