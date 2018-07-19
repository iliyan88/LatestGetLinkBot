using BobTheBot.Entities;
using System;
using System.Collections.Generic;

namespace BobTheBot.Kernel
{
    public interface IWordCache : ICache<IEnumerable<WordDto>, IReadOnlyList<SearchKey>>
    {
    }
}
