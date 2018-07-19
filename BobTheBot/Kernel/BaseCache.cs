using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace BobTheBot.Kernel
{
    public abstract class BaseCache<TCache, TStore> : ICache<TCache, TStore>
    {
        private readonly IDistributedCache distributedCache;
        private readonly string key;
        private readonly DistributedCacheEntryOptions cacheEntryOptions;
        private readonly Func<TStore, TCache> cacheSelector;
        private readonly IMemoryCache memoryCache;
        private readonly CacheInMemoryOffload cacheInMemoryOffload;
        private readonly TimeSpan inMemoryValidTo;

        // private TCache memoryData = default;
        // bool hasMemoryData = false;

        protected BaseCache(
            IDistributedCache distributedCache,
            string key,
            DistributedCacheEntryOptions cacheEntryOptions,
            Func<TStore, TCache> cacheSelector)
            : this(
                distributedCache,
                key,
                cacheEntryOptions,
                cacheSelector,
                null,
                CacheInMemoryOffload.NONE,
                TimeSpan.Zero)
        {
        }

        protected BaseCache(
            IDistributedCache distributedCache,
            string key,
            DistributedCacheEntryOptions cacheEntryOptions,
            Func<TStore, TCache> cacheSelector,
            IMemoryCache memoryCache,
            CacheInMemoryOffload cacheInMemoryOffload,
            TimeSpan inMemoryValidTo)
        {
            this.distributedCache = distributedCache;
            this.key = key;
            this.cacheEntryOptions = cacheEntryOptions;
            this.cacheSelector = cacheSelector;
            this.memoryCache = memoryCache;
            this.cacheInMemoryOffload = cacheInMemoryOffload;
            this.inMemoryValidTo = inMemoryValidTo;
            if (cacheInMemoryOffload == CacheInMemoryOffload.Scoped)
            {
                memoryCache.Remove(key);
            }
        }

        /// <inheritdoc />
        public async Task<TCache> GetAsync()
        {
            var data = await GetFromCacheAsync();
            if (data != null)
            {
                return data;
            }
            return await SetAsync();
        }

        /// <inheritdoc />
        public async Task<TCache> GetFromCacheAsync()
        {
            if (memoryCache != null && memoryCache.TryGetValue<TCache>(key, out var cached) && cached != null)
            {
                return cached;
            }
            var data = await distributedCache.GetAsAsync<TCache>(key);
            if (data != null)
            {
                SetMemoryData(data);
            }
            return data;
        }

        /// <inheritdoc />
        public abstract Task<TStore> GetFromStoreAsync();

        /// <inheritdoc />
        public async Task InvalidateAsync(DistributedCacheEntryOptions cacheOptions = null)
        {
            var data = await GetFromCacheAsync();
            if (data != null)
            {
                await SetAsync(cacheOptions);
            }
        }

        /// <inheritdoc />
        public async Task<TCache> SetAsync(DistributedCacheEntryOptions cacheOptions = null)
        {
            cacheOptions = cacheOptions ?? cacheEntryOptions;
            var clients = await GetFromStoreAsync();
            var data = cacheSelector(clients);
            if (data != null)
            {
                await distributedCache.SetAsAsync(key, data, cacheOptions);
                SetMemoryData(data);
            }
            return data;
        }

        private void SetMemoryData(TCache data)
        {
            switch (cacheInMemoryOffload)
            {
                case CacheInMemoryOffload.Scoped:
                case CacheInMemoryOffload.Singleton:
                    memoryCache.Set(key, data, inMemoryValidTo);
                    break;
            }
        }

        private void ResetMemoryData()
        {
            switch (cacheInMemoryOffload)
            {
                case CacheInMemoryOffload.Scoped:
                case CacheInMemoryOffload.Singleton:
                    memoryCache.Remove(key);
                    break;
            }
        }

        /// <inheritdoc />
        public async Task RemoveAsync()
        {
            ResetMemoryData();
            await distributedCache.RemoveAsync(key);
        }

        /// <inheritdoc />
        public async Task RefreshAsync()
        {
            await distributedCache.RefreshAsync(key);
        }
    }
}
