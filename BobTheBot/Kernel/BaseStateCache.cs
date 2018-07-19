using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace BobTheBot.Kernel
{
    public abstract class BaseStateCache<TCache, TStore, TState> : IStateCache<TCache, TStore, TState> //where T : TCache
    {
        private readonly IDistributedCache distributedCache;
        private readonly Func<TState, string> keySelector;
        private readonly DistributedCacheEntryOptions cacheEntryOptions;
        private readonly Func<TStore, TCache> cacheSelector;
        private readonly IMemoryCache memoryCache;
        private readonly CacheInMemoryOffload cacheInMemoryOffload;
        private readonly TimeSpan inMemoryValidTo;

        protected BaseStateCache(
            IDistributedCache distributedCache,
            Func<TState, string> keySelector,
            DistributedCacheEntryOptions cacheEntryOptions,
            Func<TStore, TCache> cacheSelector)
            : this(
                distributedCache,
                keySelector,
                cacheEntryOptions,
                cacheSelector,
                null,
                CacheInMemoryOffload.NONE,
                TimeSpan.Zero)
        {
        }

        protected BaseStateCache(
            IDistributedCache distributedCache,
            Func<TState, string> keySelector,
            DistributedCacheEntryOptions cacheEntryOptions,
            Func<TStore, TCache> cacheSelector,
            IMemoryCache memoryCache,
            CacheInMemoryOffload cacheInMemoryOffload,
            TimeSpan inMemoryValidTo)
        {
            this.distributedCache = distributedCache;
            this.keySelector = keySelector;
            this.cacheEntryOptions = cacheEntryOptions;
            this.cacheSelector = cacheSelector;
            this.memoryCache = memoryCache;
            this.cacheInMemoryOffload = cacheInMemoryOffload;
            this.inMemoryValidTo = inMemoryValidTo;
            this.cacheInMemoryOffload = cacheInMemoryOffload;
            this.inMemoryValidTo = inMemoryValidTo;
            if (cacheInMemoryOffload == CacheInMemoryOffload.Scoped)
            {
                inMemoryValidTo = TimeSpan.FromSeconds(2);
            }
        }

        /// <inheritdoc />
        public async Task<TCache> GetAsync(TState state)
        {
            var data = await GetFromCacheAsync(state);
            if (data != null)
            {
                return data;
            }
            return await SetAsync(state);
        }

        /// <inheritdoc />
        public async Task<TCache> GetFromCacheAsync(TState state)
        {
            var key = keySelector(state);
            if (memoryCache != null && memoryCache.TryGetValue<TCache>(key, out var cached) && cached != null)
            {
                return cached;
            }
            var data = await distributedCache.GetAsAsync<TCache>(key);
            if (data != null)
            {
                SetMemoryData(state, data);
            }
            return data;
        }

        /// <inheritdoc />
        public abstract Task<TStore> GetFromStoreAsync(TState state);

        /// <inheritdoc />
        public async Task InvalidateAsync(TState state, DistributedCacheEntryOptions cacheOptions = null)
        {
            var data = GetFromCacheAsync(state);
            if (data != null)
            {
                await SetAsync(state, cacheOptions);
            }
        }

        /// <inheritdoc />
        public async Task<TCache> SetAsync(TState state, DistributedCacheEntryOptions cacheOptions = null)
        {
            cacheOptions = cacheOptions ?? cacheEntryOptions;
            var dbData = await GetFromStoreAsync(state);
            if (dbData == null)
            {
                return default(TCache);
            }
            var data = cacheSelector(dbData);
            if (data != null)
            {
                await distributedCache.SetAsAsync(keySelector(state), data, cacheOptions);
                SetMemoryData(state, data);
            }
            return data;
        }


        private void SetMemoryData(TState state, TCache data)
        {
            var key = keySelector(state);
            switch (cacheInMemoryOffload)
            {
                case CacheInMemoryOffload.Scoped:
                case CacheInMemoryOffload.Singleton:
                    memoryCache.Set(key, data, inMemoryValidTo);
                    break;
            }
        }

        private void ResetMemoryData(TState state)
        {
            var key = keySelector(state);
            switch (cacheInMemoryOffload)
            {
                case CacheInMemoryOffload.Scoped:
                case CacheInMemoryOffload.Singleton:
                    memoryCache.Remove(key);
                    break;
            }
        }

        /// <inheritdoc />
        public async Task RemoveAsync(TState state)
        {
            ResetMemoryData(state);
            await distributedCache.RemoveAsync(keySelector(state));
        }

        /// <inheritdoc />
        public async Task RefreshAsync(TState state)
        {
            await distributedCache.RefreshAsync(keySelector(state));
        }
    }
}
