using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CafeSystem.Areas.Admin.ViewModels;

public class RegisterViewModel
{
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Surname { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; }
    
    [Required]
    public string Role { get; set; }
    
    [Required]
    [Phone]
    public string Phone { get; set; }
    
    [ValidateNever]
    public IEnumerable<string>? Roles { get; set; }
}