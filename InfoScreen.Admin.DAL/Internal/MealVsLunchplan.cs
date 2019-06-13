using InfoScreen.Admin.Logic.Entity;

namespace InfoScreen.Admin.Logic.Internal
{
    public class MealVsLunchplan
    {
        public int Id { get; }
        public int MealId { get; set; }
        public int LunchplanId { get; set; }
        public Weekday Weekday { get; set; }

        public MealVsLunchplan(int id, int mealId, int lunchplanId, Weekday weekday)
        {
            Id = id;
            MealId = mealId;
            LunchplanId = lunchplanId;
            Weekday = weekday;
        }
    }
}