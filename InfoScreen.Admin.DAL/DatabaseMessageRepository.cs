using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading.Tasks;

namespace InfoScreen.Admin.Logic
{
    public class DatabaseMessageRepository : IMessageRepository
    {
        private const string GetMessageQuery = "SELECT * from Messages WHERE Id={0}";
        private const string ListMessagesQuery = "SELECT * from Messages";
        private const string NewestMessageQuery = "SELECT TOP 1 * from Messages ORDER BY Date DESC";

        private const string CreateMessageQuery =
            "INSERT into Messages (AdminId, Date, Text, Header) VALUES (@AdminId, @Date, @Text, @Header)";

        private static Message ParseMessage(DataRow row)
        {
            return new Message(
                id: (int) row["Id"],
                createdBy: (int) row["AdminId"],
                date: (DateTime) row["Date"],
                text: (string) row["Text"],
                header: (string) row["Header"]
            );
        }

        public async Task<List<Message>> ListMessages()
        {
            var data = await Database.Query(ListMessagesQuery);
            var results = new List<Message>();
            foreach (DataRow row in data.Tables["Table"].Rows)
                results.Add(ParseMessage(row));
            return results;
        }

        public async Task<Message> GetMessage(int id)
        {
            var data = await Database.Query(string.Format(GetMessageQuery, id));
            var table = data.Tables["Table"];
            if (table.Rows.Count <= 0)
                return null;
            var row = table.Rows[0];
            return ParseMessage(row);
        }

        public async Task<Message> GetNewestMessage()
        {
            var data = await Database.Query(NewestMessageQuery);
            var table = data.Tables["Table"];
            if (table.Rows.Count <= 0)
                return null;
            var row = table.Rows[0];
            return ParseMessage(row);
        }

        public async Task<bool> CreateMessage(Message message)
        {
            await Database.Query(CreateMessageQuery, parameters: new Dictionary<string, object>
            {
                {"@AdminId", message.CreatedBy},
                {"@Date", message.Date},
                {"@Text", message.Text},
                {"@Header", message.Header}
            });
            return true;
        }
    }
}