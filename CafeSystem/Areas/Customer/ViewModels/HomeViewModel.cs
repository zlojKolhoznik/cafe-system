using CafeSystem.Models;

namespace CafeSystem.Areas.Customer.ViewModels;

public class HomeViewModel
{
    public HomeViewModel(IEnumerable<Dish> dishes, IEnumerable<Category> categories, int? selectedCategoryId = null)
    {
        Dishes = dishes;
        Categories = categories;
        SelectedCategoryId = selectedCategoryId;
    }

    public IEnumerable<Dish> Dishes { get; set; }
    
    public IEnumerable<Category> Categories { get; set; }
    
    public int? SelectedCategoryId { get; set; }
}