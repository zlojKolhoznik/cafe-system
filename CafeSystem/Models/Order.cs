using System.ComponentModel.DataAnnotations;

namespace CafeSystem.Models;

public class Order
{
    [Key]
    public int Id { get; set; }
    
    public OrderStatus Status { get; set; }
    
    public IEnumerable<OrderItem> Items { get; set; }
}

public enum OrderStatus
{
    Creating,
    Placed,
    InProcess,
    Delivering,
    CheckingOut,
    Completed
}