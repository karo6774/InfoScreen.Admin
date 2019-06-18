using System;
using GraphQL.Types;
using InfoScreen.Admin.Logic;

namespace InfoScreen.Admin.Web.Models
{
    public class InfoScreenQuery : ObjectGraphType<object>
    {
        public InfoScreenQuery(
            ILunchplanRepository lunchplans,
            IMealRepository meals,
            IMessageRepository messages,
            IAdminRepository admins
        )
        {
            Name = "Query";

            FieldAsync<AdminType>(
                "admin",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> {Name = "id"},
                    new QueryArgument<StringGraphType> {Name = "username"}
                ),
                resolve: async ctx =>
                {
                    if (ctx.HasArgument("id"))
                        return await admins.GetAdmin(ctx.GetArgument<int>("id"));
                    if (ctx.HasArgument("username"))
                        return await admins.FindByUsername(ctx.GetArgument<string>("username"));
                    return null;
                }
            );
            
            FieldAsync<LunchplanType>(
                "lunchplan",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> {Name = "week"}
                ),
                resolve: async ctx =>
                {
                    if (ctx.HasArgument("week"))
                        return await lunchplans.GetLunchplan(ctx.GetArgument<int>("week"));
                    return null;
                }
            );

            FieldAsync<MealType>(
                "meal",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> {Name = "id"}
                ),
                resolve: async ctx =>
                {
                    if (ctx.HasArgument("id"))
                        return await meals.GetMeal(ctx.GetArgument<int>("id"));
                    return null;
                }
            );

            FieldAsync<ListGraphType<MealType>>(
                "meals",
                resolve: async ctx => await meals.ListMeals()
            );

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