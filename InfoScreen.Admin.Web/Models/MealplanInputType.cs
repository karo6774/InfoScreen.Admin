using System;
using GraphQL.Types;
using InfoScreen.Admin.Logic.Entity;

namespace InfoScreen.Admin.Web.Models
{
    public class MealplanInputType : InputObjectGraphType
    {
        public MealplanInputType()
        {
            Name = "MealplanInput";

            foreach (var day in Enum.GetValues(typeof(Weekday)))
                Field<IntGraphType>(day.ToString());
        }
    }
}