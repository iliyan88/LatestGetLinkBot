using BobTheBot.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobTheBot
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
        void SaveChange();
        ISearchKeyRepository SearchKeyRepository { get; }
        IUserToReplyRepository UserToReplyRepository { get; }
    }
}
