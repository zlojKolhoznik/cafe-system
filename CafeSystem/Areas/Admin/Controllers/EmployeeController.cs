using CafeSystem.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CafeSystem.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = StaticDetails.RoleAdmin)]
public class EmployeeController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserStore<IdentityUser> _userStore;
    private readonly IUserEmailStore<IdentityUser> _emailStore;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(UserManager<IdentityUser> userManager,
        IUserStore<IdentityUser> userStore,
        RoleManager<IdentityRole> roleManager, 
        ILogger<EmployeeController> logger)
    {
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = GetEmailStore();
        _roleManager = roleManager;
        _logger = logger;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        IEnumerable<Models.Employee> employees = _userManager.Users.Select(u => (u as Models.Employee)!).ToList();
        Dictionary<Models.Employee, string> employeesRoles = new();
        foreach (var employee in employees)
        {
            string role = (await _userManager.GetRolesAsync(employee)).First();
            employeesRoles.Add(employee, role);
        }

        return View(new EmployeeViewModel { EmployeesRoles = employeesRoles });
    }

    // GET
    public IActionResult Register()
    {
        IEnumerable<string> roles = _roleManager.Roles.Select(r => r.Name).ToList();
        return View(new RegisterViewModel { Roles = roles });
    }
    
    // POST
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel vm)
    {
        if (ModelState.IsValid)
        {
            var user = (Models.Employee)CreateUser();

            await _userStore.SetUserNameAsync(user, vm.Phone, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, vm.Email, CancellationToken.None);
            user.PhoneNumber = vm.Phone;
            user.Name = vm.Name;
            user.Surname = vm.Surname;

            var result = await _userManager.CreateAsync(user, vm.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                await _userManager.AddToRoleAsync(user, vm.Role);
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        // If we got this far, something failed, redisplay form
        IEnumerable<string> roles = _roleManager.Roles.Select(r => r.Name).ToList();
        vm.Roles = roles;
        return View(vm);
    }
    
    public IActionResult Delete(string? id)
    {
        Models.Employee? user = (Models.Employee)_userManager.Users.FirstOrDefault(u => u.Id == id);
        
        if (user is null)
        {
            return NotFound();
        }
        ViewData["Role"] = _userManager.GetRolesAsync(user).Result.First();
        return View(user);
    }
    
    // POST
    [HttpPost]
    public async Task<IActionResult> Delete(Models.Employee employee)
    {
        Models.Employee? user = (Models.Employee?)_userManager.Users.FirstOrDefault(u => u.Id == employee.Id);

        if (user is null)
        {
            return NotFound();
        }
        
        IEnumerable<string> roles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, roles);
        await _userManager.DeleteAsync(user);
        return RedirectToAction("Index");
    }
    
    // GET
    public IActionResult NewRole(string? id)
    {
        Models.Employee? user = (Models.Employee)_userManager.Users.FirstOrDefault(u => u.Id == id);
        
        if (user is null)
        {
            return NotFound();
        }
        
        string role = _userManager.GetRolesAsync(user).Result.First();
        IEnumerable<string> roles = _roleManager.Roles.Select(r => r.Name).ToList();
        ViewData["Role"] = role;
        ViewData["Id"] = id;
        return View(roles);
    }
    
    // POST
    [HttpPost]
    public async Task<IActionResult> NewRole(string id, string oldRole, string newRole)
    {
        var user = (Models.Employee)_userManager.Users.FirstOrDefault(u => u.Id == id);
        
        if (user is null)
        {
            return NotFound();
        }
        
        await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRoleAsync(user, oldRole);
        await _userManager.AddToRoleAsync(user, newRole);
        return RedirectToAction("Index");
    }

    private IdentityUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<Models.Employee>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(Employee)}'. Ensure that '{nameof(Employee)}' is not an abstract class and has a parameterless constructor, or alternatively override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        }
    }

    private IUserEmailStore<IdentityUser> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<IdentityUser>)_userStore;
    }
}