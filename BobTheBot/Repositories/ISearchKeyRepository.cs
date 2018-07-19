using BobTheBot.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobTheBot.Repositories
{
    public interface ISearchKeyRepository
    {
        Task InsertAsync(SearchKey user);
        void Update(SearchKey user);
        void Delete(SearchKey user);
        Task<IReadOnlyList<SearchKey>> GetAllWords();
        Task<SearchKey> GetWordById(int id);
    }
}
