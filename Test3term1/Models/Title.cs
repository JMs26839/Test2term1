using System.ComponentModel.DataAnnotations;

namespace Test3term1.Models;

public class Title
{   [Key]
    public int Id { get; set; }
    public string Name { get; set; }
}