using GraphQL.Types;

namespace InfoScreen.Admin.Web.Models
{
    public class MealInputType : InputObjectGraphType
    {
        public MealInputType()
        {
            Name = "MealInput";
            Field<NonNullGraphType<StringGraphType>>("description");
        }
    }
}