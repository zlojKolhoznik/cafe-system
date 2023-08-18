using CafeSystem.Areas.Customer.ViewModels;
using CafeSystem.Data;
using CafeSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CafeSystem.Areas.Customer.Controllers;

//TODO: Add dish remove from cart

[Area("Customer")]
public class HomeController : Controller
{
    private readonly CafeDbContext _dbContext;

    public HomeController(CafeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // GET
    public IActionResult Index(int? id, int? selectedCategoryId)
    {
        IEnumerable<Dish> dishes = _dbContext.Dishes.Include(d => d.Category).ToList();
        IEnumerable<Category> categories = _dbContext.Categories.ToList();
        ViewData["TableNumber"] = id;
        return View(new HomeViewModel(dishes, categories, selectedCategoryId));
    }

    // POST
    [HttpPost]
    public IActionResult Index(int? tableRequest, int quantity, int dishId, int tableNumber)
    {
        if (tableRequest is not null)
        {
            return RedirectToAction("Index", new { id = (int)tableRequest });
        }

        var order = _dbContext.Orders.FirstOrDefault(o =>
            o.TableNumber == tableNumber && o.Status == OrderStatus.Creating);
        if (order is null)
        {
            order = new Order { TableNumber = tableNumber, Status = OrderStatus.Creating };
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();
        }

        var orderItem = _dbContext.OrderItems.FirstOrDefault(oi => oi.OrderId == order.Id && oi.DishId == dishId);
        if (orderItem is null)
        {
            orderItem = new OrderItem { Quantity = quantity, DishId = dishId, OrderId = order.Id };
            _dbContext.OrderItems.Add(orderItem);
            _dbContext.SaveChanges();
        }
        else
        {
            orderItem.Quantity = quantity;
            _dbContext.Orders.Update(order);
            _dbContext.SaveChanges();
        }

        return RedirectToAction("Index", "Cart",  new { id = tableNumber });
    }
}