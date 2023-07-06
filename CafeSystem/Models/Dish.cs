using System.ComponentModel.DataAnnotations;

namespace CafeSystem.Models;

public class Dish
{
    [Key] public int Id { get; set; }
    
    [Required] public string Name { get; set; }
    
    [Required] public decimal Price { get; set; }
    
    [Required] public string ImagePath { get; set; }
    
    public int CategoryId { get; set; }
    
    public Category Category { get; set; }
}