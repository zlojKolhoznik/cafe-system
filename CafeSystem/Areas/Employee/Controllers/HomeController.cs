using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CafeSystem.Areas.Employee.Controllers;

[Area("Employee")]
[Authorize(Roles = $"{StaticDetails.RoleCook}, {StaticDetails.RoleWaiter}")]
public class HomeController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Cart()
    {
        throw new NotImplementedException();
    }
}