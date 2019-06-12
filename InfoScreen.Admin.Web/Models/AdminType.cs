using GraphQL.Types;

namespace InfoScreen.Admin.Web.Models
{
    public class AdminType : ObjectGraphType<DAL.Entity.Admin>
    {
        public AdminType()
        {
            Name = "Admin";
            Field(it => it.Id);
            Field(it => it.Username);
        }
    }
}