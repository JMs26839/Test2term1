using System.ComponentModel.DataAnnotations;

namespace Test3term1.Models;

public class CharacterTitle
{   [Key]
    public int CharacterId { get; set; }
    public Character Character { get; set; }
    public int TitleId { get; set; }
    public Title Title { get; set; }
    public DateTime AcquiredAt { get; set; }
}