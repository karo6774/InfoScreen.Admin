using System;
using GraphQL.Types;
using InfoScreen.Admin.Logic;

namespace InfoScreen.Admin.Web.Models
{
    public class InfoScreenQuery : ObjectGraphType<object>
    {
        public InfoScreenQuery(IMessageRepository messages)
        {
            Name = "Query";

            FieldAsync<MessageType>(
                "message",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> {Name = "id"}
                ),
                resolve: async ctx =>
                {
                    if (ctx.HasArgument("id"))
                        return await messages.GetMessage(ctx.GetArgument<int>("id"));
                    return null;
                }
            );

            FieldAsync<ListGraphType<MessageType>>(
                "messages",
                resolve: async ctx => await messages.ListMessages()
            );

            FieldAsync<MessageType>(
                "newestMessage",
                resolve: async ctx => await messages.GetNewestMessage()
            );
        }
    }
}