using System;

namespace InfoScreen.Admin.Logic
{
    public class Message
    {
        public int Id { get; }
        public int CreatedBy { get; set;  }

        public DateTime Date { get; set; }
        public string Text { get; set; }
        public string Header { get; set; }

        public Message()
        {
            Id = -1;
        }

        public Message(int id, int createdBy, DateTime date, string text, string header)
        {
            Id = id;
            CreatedBy = createdBy;
            Date = date;
            Text = text;
            Header = header;
        }
    }
}