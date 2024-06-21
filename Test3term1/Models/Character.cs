using System.ComponentModel.DataAnnotations;

namespace Test3term1.Models;

public class Character
{
    [Key]
    public int Id { get; set; }
    [MaxLength(200)]
    public string FirstName { get; set; }
    [MaxLength(200)]
    public string LastName { get; set; }
    public int CurrWei { get; set; }
    public int MaxWeight { get; set; }
    public ICollection<Backpack> ItemsInBackpacks { get; set; }
    public ICollection<CharacterTitle> Titles { get; set; }
}








