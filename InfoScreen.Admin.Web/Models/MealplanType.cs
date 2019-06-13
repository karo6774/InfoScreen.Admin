using System;
using System.Collections.Generic;
using GraphQL.Types;
using InfoScreen.Admin.Logic.Entity;

namespace InfoScreen.Admin.Web.Models
{
    public class MealplanType : ObjectGraphType<Dictionary<Weekday, int>>
    {
        public MealplanType()
        {
            Name = "Mealplan";

            foreach (var weekday in Enum.GetValues(typeof(Weekday)))
                Field<IntGraphType>(weekday.ToString(),
                    resolve: it => it.Source.GetValueOrDefault(Enum.Parse<Weekday>(it.FieldName, true), 0));
        }
    }
}