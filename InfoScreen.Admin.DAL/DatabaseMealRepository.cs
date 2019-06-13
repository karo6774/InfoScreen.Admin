using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using InfoScreen.Admin.Logic.Entity;

namespace InfoScreen.Admin.Logic
{
    public class DatabaseMealRepository : IMealRepository
    {
        private const string GetMealQuery = "SELECT * from Meals WHERE Id=@Id";
        private const string ListMealsQuery = "SELECT * from Meals";

        private const string CreateMealQuery =
            "INSERT into Meals (TimesChosen, Description) VALUES (0, @Description)";

        private static Meal ParseMeal(DataRow row)
        {
            return new Meal(
                id: (int) row["Id"],
                timesChosen: (int) row["TimesChosen"],
                description: (string) row["Description"]
            );
        }

        public async Task<List<Meal>> ListMeals()
        {
            var data = await Database.Query(ListMealsQuery);
            var results = new List<Meal>();
            foreach (DataRow row in data.Tables["Table"].Rows)
                results.Add(ParseMeal(row));
            return results;
        }

        public async Task<Meal> GetMeal(int id)
        {
            var data = await Database.Query(GetMealQuery, parameters: new Dictionary<string, object>
            {
                {"@Id", id}
            });
            var table = data.Tables["Table"];
            if (table.Rows.Count <= 0)
                return null;
            var row = table.Rows[0];
            return ParseMeal(row);
        }

        public async Task<bool> CreateMeal(Meal meal)
        {
            await Database.Query(CreateMealQuery, parameters: new Dictionary<string, object>
            {
                {"@Description", meal.Description}
            });
            return true;
        }
    }
}