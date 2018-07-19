using BobTheBot.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobTheBot.Repositories
{
    public class SearchKeyRepository  : ISearchKeyRepository
    {
        private readonly AppDbContext dbContext;

        public SearchKeyRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task InsertAsync(SearchKey searchKey)
        {
            await dbContext.SearchKeys.AddAsync(searchKey);
        }

        public void Update(SearchKey searchKey)
        {
            dbContext.SearchKeys.Update(searchKey);
        }

        public void Delete(SearchKey searchKey)
        {
            dbContext.SearchKeys.Remove(searchKey);
        }

        public async Task<IReadOnlyList<SearchKey>> GetAllWords()
        {
            return await dbContext.SearchKeys.ToListAsync();
        }
        public async Task<SearchKey> GetWordById(int id)
        {
            return await dbContext.SearchKeys.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
