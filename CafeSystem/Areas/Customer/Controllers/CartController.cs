using CafeSystem.Data;
using CafeSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CafeSystem.Areas.Customer.Controllers;

[Area("Customer")]
public class CartController : Controller
{
    private readonly CafeDbContext _dbContext;

    public CartController(CafeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // GET
    public IActionResult Index(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var order = _dbContext.Orders.Include(o => o.Items).ThenInclude(oi => oi.Dish)
            .FirstOrDefault(o => o.TableNumber == id && o.Status == OrderStatus.Creating);
        ViewData["TableNumber"] = id;

        return View(order);
    }

    // POST
    [HttpPost]
    public IActionResult Index(int orderId)
    {
        var order = _dbContext.Orders.FirstOrDefault(o => o.Id == orderId);
        if (order is null)
        {
            return NotFound();
        }

        order.Status = OrderStatus.Placed;
        _dbContext.Orders.Update(order);
        _dbContext.SaveChanges();
        return RedirectToAction("Index", "Home", new { id = order.TableNumber });
    }

    // GET
    public IActionResult Delete(int id)
    {
        var item = _dbContext.OrderItems.Include(oi => oi.Order).FirstOrDefault(oi => oi.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        _dbContext.OrderItems.Remove(item);
        _dbContext.SaveChanges();
        return RedirectToAction("Index", new { id = item.Order.TableNumber });
    }

    // GET
    [HttpGet]
    public IActionResult Payment(int id)
    {
        IEnumerable<Order> orders = _dbContext.Orders.Include(o => o.Items).ThenInclude(oi => oi.Dish)
            .Where(o => o.TableNumber == id && (int)o.Status >= (int)OrderStatus.Placed && !o.IsPaymentDone).ToList();

        ViewData["TableNumber"] = id;
        return View(orders);
    }

    // POST
    [HttpPost]
    public IActionResult Payment(int? orderId)
    {
        // Here we are assuming that the payment is done successfully
        var order = _dbContext.Orders.FirstOrDefault(o => o.Id == orderId);
        if (order is null)
        {
            return NotFound();
        }

        order.IsPaymentDone = true;
        if (order.Status == OrderStatus.CheckingOut)
        {
            order.Status = OrderStatus.Completed;
        }

        _dbContext.Orders.Update(order);
        _dbContext.SaveChanges();
        return RedirectToAction("Index", "Home", new { id = order.TableNumber });
    }
}