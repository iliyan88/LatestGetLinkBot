using BobTheBot.Entities;
using BobTheBot.Kernel;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BobTheBot.Cache
{
    public class WordCache : BaseCache<IEnumerable<WordDto>, IReadOnlyList<SearchKey>>, IWordCache
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<WordCache> logger;
        private readonly ISystemClock systemClock;

        public WordCache(
            IUnitOfWork unitOfWork,
            ILogger<WordCache> logger,
            IDistributedCache distributedCache,
            ISystemClock systemClock,
            //ApplicationSettings applicationSettings,
            IMemoryCache memoryCache)
            : base(
                distributedCache,
                "Words",
                new DistributedCacheEntryOptions()
                {
                    AbsoluteExpiration = systemClock.UtcNow.Add(TimeSpan.FromDays(1)),
                },
                (words) => words.Select(x => new WordDto(x.Word)),
                memoryCache,
                CacheInMemoryOffload.Singleton,
                TimeSpan.FromMinutes(1)
            )
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
            this.systemClock = systemClock;
        }

        public override async Task<IReadOnlyList<SearchKey>> GetFromStoreAsync()
        {
            return await unitOfWork.SearchKeyRepository.GetAllWords();
        }
    }
}
