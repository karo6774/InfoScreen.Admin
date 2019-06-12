using GraphQL.Types;

namespace InfoScreen.Admin.Web.Models
{
    public class MessageInputType : InputObjectGraphType
    {
        public MessageInputType()
        {
            Name = "MessageInput";
            Field<NonNullGraphType<StringGraphType>>("text");
            Field<NonNullGraphType<StringGraphType>>("header");
        }
    }
}