using CafeSystem.Data;
using CafeSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace CafeSystem.Areas.Admin.Controllers;

[Area("Admin")]
public class CategoryController : Controller
{
    private CafeDbContext _dbContext;

    public CategoryController(CafeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // GET
    public IActionResult Index()
    {
        IEnumerable<Category> categories = _dbContext.Categories.AsEnumerable();
        return View(categories);
    }
    
    // GET
    public IActionResult Upsert(int? id)
    {
        var category = new Category();
        if (id is not null)
        {
            category = _dbContext.Categories.FirstOrDefault(c => c.Id == id);
        }

        if (category is null)
        {
            ViewData["error"] = "Category not in database";
            return RedirectToAction("Index");
        }
        
        return View(category);
    }
    
    // POST
    [HttpPost]
    public IActionResult Upsert(Category category)
    {
        if (!ModelState.IsValid)
        {
            return View(category);
        }

        if (category.Id != 0)
        {
            _dbContext.Categories.Update(category);
            ViewData["success"] = "Category successfully updated";
        }
        else
        {
            _dbContext.Categories.Add(category);
            ViewData["success"] = "Category successfully added";
        }

        _dbContext.SaveChanges();

        return RedirectToAction("Index");
    }
    
    // GET
    public IActionResult Delete(int? id)
    {
        if (id is null or 0)
        {
            ViewData["error"] = "Unknown error ocurred";
            return RedirectToAction("Index");
        }

        Category? category = _dbContext.Categories.FirstOrDefault(c => c.Id == id);
        if (category is null)
        {
            ViewData["error"] = "Category not in database";
            return RedirectToAction("Index");
        }

        return View(category);
    }
    
    // POST
    [HttpPost]
    public IActionResult Delete(Category? category)
    {
        if (category is null)
        {
            ViewData["error"] = "Unknown error ocurred";
            return RedirectToAction("Index");
        }
        _dbContext.Categories.Remove(category);
        _dbContext.SaveChanges();
        return RedirectToAction("Index");
    }
}