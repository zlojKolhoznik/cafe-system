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

    public bool IsPaymentDone { get; set; } = false;

    public OrderStatus Status { get; set; }

    public decimal Total
    {
        get
        {
            return Items.Sum(item => item.Quantity * item.Dish.Price);
        }
    }
    
    public IEnumerable<OrderItem> Items { get; set; }
    
    public IEnumerable<Employee> Employees { get; set; }
}

public enum OrderStatus
{
    Creating,
    Placed,
    Preparing,
    FoodReady,
    Delivering,
    CheckingOut,
    Completed
}