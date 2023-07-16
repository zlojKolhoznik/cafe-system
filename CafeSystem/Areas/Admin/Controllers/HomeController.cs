using CafeSystem.Data;
using CafeSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CafeSystem.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = StaticDetails.RoleAdmin)]
public class HomeController : Controller
{

    // GET
    public IActionResult Index()
    {
        return View();
    }
}