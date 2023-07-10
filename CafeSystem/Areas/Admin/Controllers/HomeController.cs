using CafeSystem.Data;
using CafeSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace CafeSystem.Areas.Admin.Controllers;

[Area("Admin")]
public class HomeController : Controller
{

    // GET
    public IActionResult Index()
    {
        return View();
    }
}