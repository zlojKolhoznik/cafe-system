using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CafeSystem.Models;

public class Order
{
    [Key]
    public int Id { get; set; }

    [Range(1, 50)]
    [DisplayName("Table")]
    public int TableNumber { get; set; }
    
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