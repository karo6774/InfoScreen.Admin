using GraphQL.Types;
using InfoScreen.Admin.Logic.Entity;

namespace InfoScreen.Admin.Web.Models
{
    public class LunchplanType : ObjectGraphType<Lunchplan>
    {
        public LunchplanType()
        {
            Name = "Lunchplan";
            Field(it => it.Id);
            Field(it => it.WeekNumber);
            Field<MealplanType>("Mealplan", resolve: it => it.Source.Mealplan);
        }
    }
}