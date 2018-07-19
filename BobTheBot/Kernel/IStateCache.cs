using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;

namespace BobTheBot.Kernel
{
    public interface IStateCache<TCache, TStore, TState>
    {
        /// <summary>
        /// Get from cache and if there is no cache, data is requested from the store then cached and returned
        /// </summary>
        /// <param name="state"></param>
        /// <returns>Returns the cached data</returns>
        Task<TCache> GetAsync(TState state);

        /// <summary>
        /// Get directly from cache
        /// </summary>
        /// <param name="state"></param>
        /// <returns>Returns the cache data or null if there is no cache</returns>
        Task<TCache> GetFromCacheAsync(TState state);

        /// <summary>
        /// Get from store (no cache)
        /// </summary>
        /// <param name="state"></param>
        /// <returns>Returns data from store</returns>
        Task<TStore> GetFromStoreAsync(TState state);

        /// <summary>
        /// Refreshes data in the cache based on its key, resetting its sliding expiration timeout (if any).
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        Task RefreshAsync(TState state);

        /// <summary>
        /// Removes a cache entry based on its key.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        Task RemoveAsync(TState state);

        /// <summary>
        /// Data is requested from the store then cached
        /// </summary>
        /// <param name="state"></param>
        /// <param name="cacheOptions"></param>
        /// <returns>Returns the cached data</returns>
        Task<TCache> SetAsync(TState state, DistributedCacheEntryOptions cacheOptions = null);

        /// <summary>
        /// If data exist it is overridden with the most recent data from the store otherwise no action is done
        /// </summary>
        /// <param name="state"></param>
        /// <param name="cacheOptions"></param>
        /// <returns></returns>
        Task InvalidateAsync(TState state, DistributedCacheEntryOptions cacheOptions = null);
    }
}
