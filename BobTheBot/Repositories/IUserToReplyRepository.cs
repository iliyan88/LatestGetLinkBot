using BobTheBot.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobTheBot.Repositories
{
    public interface IUserToReplyRepository
    {
        Task<UserToReply> GetByIDAsync(int id);
        Task<IReadOnlyList<UserToReply>> GetAsync();
        Task InsertAsync(UserToReply user);
        void Update(UserToReply user);
        void Delete(UserToReply user);
        Task<IReadOnlyList<UserToReply>> GetActiveUsers();
        Task<UserToReply> GetUserByIdAndName(string skypeUserId, string skypeName);
    }
}
