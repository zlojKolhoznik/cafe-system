using CafeSystem.Data;
using CafeSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CafeSystem.Areas.Employee.Controllers;

[Area("Employee")]
[Authorize(Roles = $"{StaticDetails.RoleCook}, {StaticDetails.RoleWaiter}")]
public class HomeController : Controller
{
    private readonly CafeDbContext _dbContext;
    private readonly UserManager<IdentityUser> _userManager;

    public HomeController(CafeDbContext dbContext, UserManager<IdentityUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    // GET
    public IActionResult Index()
    {
        IEnumerable<Order> availableOrders = _dbContext.Orders
            .Include(order => order.Items)
            .Include(order => order.Employees);

        if (User.IsInRole(StaticDetails.RoleCook))
        {
            availableOrders = availableOrders.Where(order => order.Status == OrderStatus.Placed);
        }
        else if (User.IsInRole(StaticDetails.RoleWaiter))
        {
            availableOrders = availableOrders.Where(order => order.Status == OrderStatus.FoodReady);
        }

        return View(availableOrders);
    }
    
    // GET
    public IActionResult Take(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }
        
        Order? order = _dbContext.Orders
            .Include(order => order.Employees)
            .FirstOrDefault(order => order.Id == id);

        if (order is null)
        {
            return NotFound();
        }
        
        order.Employees = order.Employees.Append(_dbContext.Employees.First(e => e.Id == _userManager.GetUserId(User))).ToList();
        order.Status = User.IsInRole(StaticDetails.RoleCook) ? OrderStatus.Preparing : OrderStatus.Delivering;
        
        _dbContext.Orders.Update(order);
        _dbContext.SaveChanges();
        
        return RedirectToAction("ActiveOrders");
    }
    
    // GET
    public IActionResult Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }
        
        Order? order = _dbContext.Orders
            .Include(order => order.Items)
            .ThenInclude(oi => oi.Dish)
            .FirstOrDefault(order => order.Id == id);

        if (order is null)
        {
            return NotFound();
        }
        
        return View(order);
    }
    
    // GET
    public IActionResult ActiveOrders()
    {
        IEnumerable<Order> activeOrders = _dbContext.Orders
            .Include(order => order.Items)
            .Include(order => order.Employees);
        
        if (User.IsInRole(StaticDetails.RoleCook))
        {
            activeOrders = activeOrders.Where(order => order.Status == OrderStatus.Preparing);
        }
        else if (User.IsInRole(StaticDetails.RoleWaiter))
        {
            activeOrders = activeOrders.Where(order => order.Status == OrderStatus.Delivering);
        }

        return View(activeOrders);
    }
    
    // GET
    public IActionResult PromoteOrder(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }
        
        Order? order = _dbContext.Orders
            .Include(order => order.Employees)
            .FirstOrDefault(order => order.Id == id);

        if (order is null)
        {
            return NotFound();
        }
        
        order.Status = User.IsInRole(StaticDetails.RoleCook) ? OrderStatus.FoodReady : OrderStatus.CheckingOut;
        if (order.IsPaymentDone && order.Status == OrderStatus.CheckingOut)
        {
            order.Status = OrderStatus.Completed;
        }
        
        _dbContext.Orders.Update(order);
        _dbContext.SaveChanges();
        
        return RedirectToAction("ActiveOrders");
    }
}