using CafeSystem.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CafeSystem.Areas.Admin.ViewModels;

public class DishViewModel
{
    public Dish Dish { get; set; }

    [ValidateNever]
    public IEnumerable<SelectListItem> Categories { get; set; }
}