using System;
using GraphQL.Types;
using InfoScreen.Admin.Logic;

namespace InfoScreen.Admin.Web.Models
{
    public class InfoScreenMutation : ObjectGraphType
    {
        public InfoScreenMutation(IMessageRepository messages)
        {
            Name = "Query";
            
            FieldAsync<BooleanGraphType>(
                "createMessage",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<MessageInputType>> {Name = "message"}
                ),
                resolve: async ctx =>
                {
                    var message = ctx.GetArgument<Message>("message");
                    message.Date = DateTime.Now;
                    // TODO: Get Admin ID from currently signed in User
                    message.CreatedBy = 1;
                    return await messages.CreateMessage(message);
                }
            );
        }
    }
}