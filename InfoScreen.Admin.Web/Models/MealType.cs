using GraphQL.Types;
using InfoScreen.Admin.Logic.Entity;

namespace InfoScreen.Admin.Web.Models
{
    public class MealType : ObjectGraphType<Meal>
    {
        public MealType()
        {
            Name = "Meal";
            Field(it => it.Id);
            Field(it => it.TimesChosen);
            Field(it => it.Description);
        }
    }
}