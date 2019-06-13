using System.Collections.Generic;

namespace InfoScreen.Admin.Logic.Entity
{
    public class Lunchplan
    {
        public int Id { get; }
        public int WeekNumber { get; set; }
        public Dictionary<Weekday, int> Mealplan { get; set; } = new Dictionary<Weekday, int>();

        public Lunchplan()
        {
        }
        
        public Lunchplan(int id, int weekNumber)
        {
            Id = id;
            WeekNumber = weekNumber;
        }
    }
}