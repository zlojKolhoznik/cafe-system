using CafeSystem.Areas.Admin.ViewModels;
using CafeSystem.Data;
using CafeSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CafeSystem.Areas.Admin.Controllers;

[Area("Admin")]
public class DishController : Controller
{
    private CafeDbContext _dbContext;
    private IWebHostEnvironment _webHostEnvironment;

    public DishController(CafeDbContext dbContext, IWebHostEnvironment webHostEnvironment)
    {
        _dbContext = dbContext;
        _webHostEnvironment = webHostEnvironment;
    }

    // GET
    public IActionResult Index()
    {
        IEnumerable<Dish> dishes = _dbContext.Dishes.Include(d => d.Category).AsEnumerable();
        return View(dishes);
    }
    
    // GET
    public IActionResult Upsert(int? id)
    {
        var dish = new Dish();
        if (id is not null)
        {
            dish = _dbContext.Dishes.FirstOrDefault(d => d.Id == id);
        }

        if (dish is null)
        {
            ViewData["error"] = "Dish not in database";
            return RedirectToAction("Index");
        }

        IEnumerable<Category> categories = _dbContext.Categories.AsEnumerable();

        var vm = new DishViewModel { Dish = dish, Categories = categories.Select(c => new SelectListItem(c.Name, c.Id.ToString())) };
        
        return View(vm);
    }
    
    // POST
    [HttpPost]
    public IActionResult Upsert(DishViewModel viewModel, IFormFile? image)
    {
        Dish dish = viewModel.Dish;

        if (ModelState.IsValid)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (image is not null)
            {
                string filename = Guid.NewGuid() + Path.GetExtension(image.FileName);
                string imagesPath = Path.Combine(wwwRootPath, @"assets\images");
                if (!string.IsNullOrEmpty(dish.ImagePath))
                {
                    string oldImage = dish.ImagePath;
                    if (System.IO.File.Exists(oldImage))
                    {
                        System.IO.File.Delete(oldImage);
                    }
                }

                using var fs = new FileStream(Path.Combine(imagesPath, filename), FileMode.Create);
                image.CopyTo(fs);
                dish.ImagePath = $@"assets\images\{filename}";
            }

            if (dish.Id != 0)
            {
                ViewData["success"] = "Dish edited successfully";
                _dbContext.Dishes.Update(dish);
            }
            else
            {
                ViewData["success"] = "Dish added successfully";
                _dbContext.Dishes.Add(dish);
            }

            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        viewModel.Categories = _dbContext.Categories.AsEnumerable().Select(c => new SelectListItem(c.Name, c.Id.ToString()));
        return View(viewModel);
    }
    
    // GET
    public IActionResult Delete(int? id)
    {
        if (id is null)
        {
            ViewData["error"] = "Dish not in database";
            return RedirectToAction("Index");
        }

        var dish = _dbContext.Dishes.Include(d => d.Category).FirstOrDefault(d => d.Id == id);
        if (dish is null)
        {
            ViewData["error"] = "Dish not in database";
            return RedirectToAction("Index");
        }

        return View(dish);
    }
    
    // POST
    [HttpPost]
    public IActionResult Delete(Dish dish)
    {
        if (dish.Id != 0)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (System.IO.File.Exists(Path.Combine(wwwRootPath, dish.ImagePath)))
            {
                System.IO.File.Delete(Path.Combine(wwwRootPath, dish.ImagePath));
            }
            _dbContext.Dishes.Remove(dish);
            _dbContext.SaveChanges();
            ViewData["success"] = "Dish deleted successfully";
        }
        else
        {
            ViewData["error"] = "Dish not in database";
        }

        return RedirectToAction("Index");
    }
}