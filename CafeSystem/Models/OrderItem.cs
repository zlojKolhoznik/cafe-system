using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CafeSystem.Models;

public class OrderItem
{
    [Key] 
    public int Id { get; set; }

    public int Quantity { get; set; }

    public int DishId { get; set; }

    [ForeignKey("DishId")]
    public Dish Dish { get; set; }

    public int OrderId { get; set; }
    
    [ForeignKey("OrderId")] 
    public Order Order { get; set; }
}