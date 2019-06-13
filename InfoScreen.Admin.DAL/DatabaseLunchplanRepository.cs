using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using InfoScreen.Admin.Logic.Entity;
using InfoScreen.Admin.Logic.Internal;

namespace InfoScreen.Admin.Logic
{
    public class DatabaseLunchplanRepository : ILunchplanRepository
    {
        private const string GetLunchplanForWeek = "SELECT * from LunchPlans WHERE Week=@Week";
        private const string GetMealVsLunchplans = "SELECT * from MealsVsLunchPlans WHERE LunchPlanId=@LunchplanId";

        private static Lunchplan ParseLunchplan(DataRow row)
        {
            return new Lunchplan(
                id: (int) row["Id"],
                weekNumber: (int) row["Week"]
            );
        }

        private static List<MealVsLunchplan> ParseMealsVsLunchplans(DataTable table)
        {
            var result = new List<MealVsLunchplan>();
            for (var i = 0; i < table.Rows.Count; i++)
            {
                var row = table.Rows[i];
                result.Add(new MealVsLunchplan(
                    id: (int) row["Id"],
                    mealId: (int) row["MealId"],
                    lunchplanId: (int) row["LunchPlanId"],
                    weekday: Enum.Parse<Weekday>((string) row["Weekday"])
                ));
            }

            return result;
        }

        public async Task<Lunchplan> GetLunchplan(int week)
        {
            var data = await Database.Query(GetLunchplanForWeek, parameters: new Dictionary<string, object>
            {
                {"@Week", week}
            });
            var table = data.Tables["Table"];
            if (table.Rows.Count <= 0)
                return null;
            var row = table.Rows[0];
            var plan = ParseLunchplan(row);

            data = await Database.Query(GetMealVsLunchplans, parameters: new Dictionary<string, object>
            {
                {"LunchplanId", plan.Id}
            });
            table = data.Tables["Table"];
            ParseMealsVsLunchplans(table)
                .ForEach(it => plan.Mealplan[it.Weekday] = it.MealId);
            return plan;
        }
    }
}