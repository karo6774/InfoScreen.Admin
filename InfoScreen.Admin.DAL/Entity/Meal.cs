namespace InfoScreen.Admin.Logic.Entity
{
    public class Meal
    {
        public int Id { get; }
        public int TimesChosen { get; set; }
        public string Description { get; set; }

        public Meal()
        {
            Id = -1;
        }

        public Meal(int id, int timesChosen, string description)
        {
            Id = id;
            TimesChosen = timesChosen;
            Description = description;
        }
    }
}