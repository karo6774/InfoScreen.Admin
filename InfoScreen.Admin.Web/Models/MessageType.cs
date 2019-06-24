using System;
using GraphQL.Types;
using InfoScreen.Admin.Logic;

namespace InfoScreen.Admin.Web.Models
{
    public class MessageType : ObjectGraphType<Message>
    {
        public MessageType(IAdminRepository admins)
        {
            Field(it => it.Id);
            Field(it => it.Date);
            FieldAsync<AdminType>(
                "createdBy",
                resolve: async ctx => await admins.GetAdmin(ctx.Source.CreatedBy));
            Field(it => it.Header);
            Field(it => it.Text);
        }
    }
}