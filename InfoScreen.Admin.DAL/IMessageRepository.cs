using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfoScreen.Admin.Logic
{
    public interface IMessageRepository
    {
        Task<List<Message>> ListMessages();

        Task<Message> GetMessage(int id);

        Task<Message> GetNewestMessage();

        Task<bool> CreateMessage(Message message);
    }
}