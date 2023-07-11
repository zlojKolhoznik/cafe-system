using CafeSystem.Data;
using CafeSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace CafeSystem.Areas.Admin.Controllers;

[Area("Admin")]
public class EmployeeController : Controller
{
    private CafeDbContext _dbContext;

    public EmployeeController(CafeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // GET
    public IActionResult Index()
    {
        IEnumerable<Employee> employees = _dbContext.Employees.AsEnumerable();
        return View(employees);
    }
    
    // GET
    public IActionResult Upsert(string? id)
    {
        var employee = new Employee();
        if (id is not null)
        {
            employee = _dbContext.Employees.FirstOrDefault(e => e.Id == id);
        }

        if (employee is null)
        {
            ViewData["error"] = "Employee not found";
            return RedirectToAction("Index");
        }
        
        return View(employee);
    }
    
    // POST
    [HttpPost]
    public IActionResult Upsert(Employee employee)
    {
        if (!ModelState.IsValid)
        {
            return View(employee);
        }
        
        if (string.IsNullOrEmpty(employee.Id))
        {
            _dbContext.Employees.Add(employee);
            ViewData["success"] = "Employee added successfully";
        }
        else
        {
            _dbContext.Employees.Update(employee);
            ViewData["success"] = "Employee updated successfully";
        }
        
        _dbContext.SaveChanges();
        return RedirectToAction("Index");
    }
    
    // GET
    public IActionResult Delete(string? id)
    {
        if (id is null)
        {
            ViewData["error"] = "Employee not found";
            return RedirectToAction("Index");
        }
        
        Employee? employee = _dbContext.Employees.FirstOrDefault(e => e.Id == id);
        if (employee is null)
        {
            ViewData["error"] = "Employee not found";
            return RedirectToAction("Index");
        }

        return View(employee);
    }
    
    // POST
    [HttpPost]
    public IActionResult Delete(Employee employee)
    {
        if (string.IsNullOrEmpty(employee.Id))
        {
            ViewData["error"] = "Employee not found";
            return RedirectToAction("Index");
        }
        
        _dbContext.Employees.Remove(employee);
        _dbContext.SaveChanges();
        ViewData["success"] = "Employee deleted successfully";
        return RedirectToAction("Index");
    }
}