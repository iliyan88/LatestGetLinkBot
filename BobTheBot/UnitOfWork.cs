using System;
using Microsoft.Extensions.DependencyInjection;
using BobTheBot.Repositories;
using System.Threading.Tasks;

namespace BobTheBot
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly AppDbContext dbContext;
        private readonly Lazy<ISearchKeyRepository> searchKeyRepository;
        private readonly Lazy<IUserToReplyRepository> userToReplyRepository;

        public ISearchKeyRepository SearchKeyRepository { get => searchKeyRepository.Value; }
        public IUserToReplyRepository UserToReplyRepository { get => userToReplyRepository.Value; }

        public UnitOfWork(AppDbContext dbContext, IServiceProvider serviceProvider)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            searchKeyRepository = new Lazy<ISearchKeyRepository>(() => serviceProvider.GetService<ISearchKeyRepository>());
            userToReplyRepository = new Lazy<IUserToReplyRepository>(() => serviceProvider.GetService<IUserToReplyRepository>());
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public void SaveChange()
        {
            dbContext.SaveChanges();
        }
    }
}
