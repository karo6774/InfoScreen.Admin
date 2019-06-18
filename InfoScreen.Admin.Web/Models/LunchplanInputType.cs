using GraphQL.Types;
using InfoScreen.Admin.Logic.Entity;

namespace InfoScreen.Admin.Web.Models
{
    public class LunchplanInputType : InputObjectGraphType<Lunchplan>
    {
        public LunchplanInputType()
        {
            Name = "LunchplanInput";
            Field<NonNullGraphType<IntGraphType>>("Week");
            Field<NonNullGraphType<MealplanInputType>>("Mealplan");
        }
    }
}