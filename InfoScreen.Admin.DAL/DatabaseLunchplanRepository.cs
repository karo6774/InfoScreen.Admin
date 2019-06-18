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
        private const string CreateLunchplanQuery = "INSERT into LunchPlans (Week) VALUES (@Week)";

        private const string GetMealVsLunchplansQuery =
            "SELECT * from MealsVsLunchPlans WHERE LunchPlanId=@LunchplanId";

        private const string UpdateMealVsLunchplanQuery = "UPDATE MealsVsLunchPlans SET MealId = @MealId WHERE Id=@Id";

        private const string RemoveMealVsLunchplanQuery =
            "DELETE from MealsVsLunchPlans WHERE LunchPlanId=@LunchplanId AND Weekday=@Weekday";

        private const string CreateMealVsLunchplanQuery =
            "INSERT into MealsVsLunchPlans (LunchPlanId, MealId, Weekday) VALUES (@LunchplanId, @MealId, @Weekday)";

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
            var plan = await GetRawLunchplan(week);
            if (plan == null)
                return null;

            var data = await Database.Query(GetMealVsLunchplansQuery, parameters: new Dictionary<string, object>
            {
                {"LunchplanId", plan.Id}
            });
            var table = data.Tables["Table"];
            ParseMealsVsLunchplans(table)
                .ForEach(it => plan.Mealplan[it.Weekday] = it.MealId);
            return plan;
        }

        private async Task<Lunchplan> GetRawLunchplan(int week)
        {
            var data = await Database.Query(GetLunchplanForWeek, parameters: new Dictionary<string, object>
            {
                {"@Week", week}
            });
            var table = data.Tables["Table"];
            if (table.Rows.Count <= 0)
                return null;
            var row = table.Rows[0];
            return ParseLunchplan(row);
        }

        public async Task<bool> SaveLunchplan(Lunchplan plan)
        {
            int lpId = plan.Id;
            bool exists = true;

            // if plan.Id is valid, assume it is already present in the db
            if (lpId <= 0)
            {
                // get id of existing lunchplan for the given week, if available
                var existingPlan = await GetRawLunchplan(plan.WeekNumber);
                if (existingPlan == null)
                {
                    await Database.Query(CreateLunchplanQuery, parameters: new Dictionary<string, object>
                    {
                        {"@Week", plan.WeekNumber}
                    });
                    existingPlan = await GetRawLunchplan(plan.WeekNumber);
                    exists = false;
                }

                lpId = existingPlan.Id;
            }

            List<MealVsLunchplan> mvlps;
            if (!exists)
                mvlps = new List<MealVsLunchplan>();
            else
            {
                var mvlpData = await Database.Query(GetMealVsLunchplansQuery, parameters: new Dictionary<string, object>
                {
                    {"@LunchplanId", lpId}
                });
                mvlps = ParseMealsVsLunchplans(mvlpData.Tables["Table"]);
            }

            foreach (Weekday day in Enum.GetValues(typeof(Weekday)))
            {
                int mealId;
                // if an entry is available, update the MVLP for that day
                if (plan.Mealplan.TryGetValue(day, out mealId))
                {
                    // if mealId is set to an invalid ID, remove any existing MVLPs for the day
                    if (mealId <= 0)
                    {
                        // only remove MVLPs if the lunchplan was already in the DB before this function call
                        if (exists)
                        {
                            await Database.Query(RemoveMealVsLunchplanQuery, parameters: new Dictionary<string, object>
                            {
                                {"@LunchplanId", lpId},
                                {"@Weekday", day.ToString()}
                            });
                        }
                    }
                    // valid mealId, either update an existing MVLP or create a new one
                    else
                    {
                        var existing = mvlps.Find(it => it.Weekday == day);
                        if (existing != null)
                        {
                            existing.MealId = mealId;
                            await Database.Query(UpdateMealVsLunchplanQuery, parameters: new Dictionary<string, object>
                            {
                                {"@MealId", mealId},
                                {"@Id", existing.Id}
                            });
                        }
                        else
                        {
                            await Database.Query(CreateMealVsLunchplanQuery, parameters: new Dictionary<string, object>
                            {
                                {"@LunchplanId", lpId},
                                {"@MealId", mealId},
                                {"@Weekday", day.ToString()}
                            });
                        }
                    }
                }
            }

            return true;
        }
    }
}