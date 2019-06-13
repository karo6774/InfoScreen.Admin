using System.Collections.Generic;
using System.Threading.Tasks;
using InfoScreen.Admin.Logic.Entity;

namespace InfoScreen.Admin.Logic
{
    public interface IMealRepository
    {
        Task<List<Meal>> ListMeals();
        
        Task<Meal> GetMeal(int id);

        Task<bool> CreateMeal(Meal meal);
    }
}