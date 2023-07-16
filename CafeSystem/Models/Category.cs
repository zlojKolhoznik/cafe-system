using System.ComponentModel.DataAnnotations;

namespace CafeSystem.Models;

public class Category
{
    [Key] 
    public int Id { get; set; }
    
    [Required] 
    public string Name { get; set; }

    public override string ToString()
    {
        return Name;
    }
}