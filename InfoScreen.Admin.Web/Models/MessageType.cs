using GraphQL.Types;
using InfoScreen.Admin.Logic;

namespace InfoScreen.Admin.Web.Models
{
    public class MessageType : ObjectGraphType<Message>
    {
        public MessageType()
        {
            Field(it => it.Id);
            Field(it => it.Date);
            Field(it => it.CreatedBy);
            Field(it => it.Header);
            Field(it => it.Text);
        }
    }
}