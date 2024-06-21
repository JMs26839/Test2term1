using System.ComponentModel.DataAnnotations;

namespace Test3term1.Models;

public class Item
{   [Key]
    public int Id { get; set; }
    [MaxLength(200)]
    public string Name { get; set; }
    public int Weight { get; set; }
}