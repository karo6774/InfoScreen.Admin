using GraphQL.Types;

namespace InfoScreen.Admin.Web.Models
{
    public class AdminInputType : InputObjectGraphType
    {
        public AdminInputType()
        {
            Name = "AdminInput";
            Field<NonNullGraphType<StringGraphType>>("username");
            Field<NonNullGraphType<StringGraphType>>("password");
        }
    }
}