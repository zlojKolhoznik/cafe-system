using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CafeSystem.Models;

public class Dish
{
    [Key] public int Id { get; set; }

    [Required] public string Name { get; set; }

    [Required] public decimal Price { get; set; }

    [ValidateNever] public string ImagePath { get; set; }

    public int CategoryId { get; set; }

    [ValidateNever] public Category Category { get; set; }
    [ValidateNever] public IEnumerable<OrderItem> OrderItems { get; set; }
}