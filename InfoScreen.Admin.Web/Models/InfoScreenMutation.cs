using System;
using System.Collections.Generic;
using GraphQL;
using GraphQL.Types;
using InfoScreen.Admin.Logic;
using InfoScreen.Admin.Logic.Entity;

namespace InfoScreen.Admin.Web.Models
{
    public class InfoScreenMutation : ObjectGraphType
    {
        public InfoScreenMutation(
            IAdminRepository admins,
            ILunchplanRepository lunchplans,
            IMealRepository meals,
            IMessageRepository messages
        )
        {
            Name = "Mutation";

            FieldAsync<BooleanGraphType>(
                "createAdmin",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<AdminInputType>> {Name = "admin"}
                ),
                resolve: async ctx =>
                {
                    var input = ctx.GetArgument<Dictionary<string, object>>("admin");
                    var admin = new DAL.Entity.Admin
                    {
                        Username = (string) input["username"]
                    };
                    admin.SetPassword((string) input["password"]);

                    return await admins.CreateAdmin(admin);
                }
            );

            FieldAsync<BooleanGraphType>(
                "saveLunchplan",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<LunchplanInputType>> {Name = "lunchplan"}
                ),
                resolve: async ctx =>
                {
                    var arg = ctx.GetArgument<Dictionary<string, object>>("lunchplan");
                    var week = (int) arg["week"];
                    var mp = (Dictionary<string, object>) arg["mealplan"];
                    var mealplan = new Dictionary<Weekday, int>();
                    foreach (var (key, val) in mp)
                        mealplan[Enum.Parse<Weekday>(key, true)] = (int) val;

                    var lunchplan = new Lunchplan
                    {
                        WeekNumber = week,
                        Mealplan = mealplan
                    };

                    return await lunchplans.SaveLunchplan(lunchplan);
                }
            );

            FieldAsync<BooleanGraphType>(
                "createMeal",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<MealInputType>> {Name = "meal"}
                ),
                resolve: async ctx =>
                {
                    var meal = ctx.GetArgument<Meal>("meal");
                    return await meals.CreateMeal(meal);
                }
            );

            FieldAsync<BooleanGraphType>(
                "createMessage",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<MessageInputType>> {Name = "message"}
                ),
                resolve: async ctx =>
                {
                    var message = ctx.GetArgument<Message>("message");
                    message.Date = DateTime.Now;
                    message.CreatedBy = ctx.UserContext.As<InfoScreenUserContext>().AdminId;
                    return await messages.CreateMessage(message);
                }
            );
        }
    }
}