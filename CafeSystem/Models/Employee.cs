using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CafeSystem.Models;

public class Employee : IdentityUser
{
    public string Name { get; set; }

    public string Surname { get; set; }
    
    public string Password { get; set; }
    
    public Role Role { get; set; }

    [ValidateNever] public IEnumerable<Order> Orders { get; set; }
}

public enum Role
{
    Admin,
    Waiter,
    Cook
}