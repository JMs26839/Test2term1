using System.ComponentModel.DataAnnotations;

namespace Test3term1.Models;

public class Backpack
{   [Key]
    public int CharId { get; set; }
    public Character Char { get; set; }
    public int ItemId { get; set; }
    public Item Item { get; set; }
    public int Amount { get; set; }
}