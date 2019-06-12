using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfoScreen.Admin.Logic
{
    /*public class InMemoryMessageRepository : IMessageRepository
    {
        public List<Message> Messages = new List<Message>();

        public async Task<List<Message>> ListMessages() => Messages;

        public async Task<Message> GetMessage(int id) => Messages.Find(it => it.Id == id);

        public async Task<Message> MessageAt(DateTime day) =>
            Messages.Find(it => it.Date >= day && it.Date.Add(TimeSpan.FromDays(1)) < day);

        public async Task UpdateMessage(Message message)
        {
            // remove existing messages, return if no previous message existed
            if (Messages.RemoveAll(it => it.Id == message.Id) <= 0)
                return;
            Messages.Add(message);
        }

        public async Task CreateMessage(Message message) => Messages.Add(message);
    }*/
}