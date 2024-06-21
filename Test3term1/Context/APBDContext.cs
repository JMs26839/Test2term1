using Microsoft.EntityFrameworkCore;
using Test3term1.Models;

namespace Test3term1.Context;

public class APBDContext : DbContext
{
    public APBDContext()
    {

    }


    public APBDContext(DbContextOptions<APBDContext> options)
        : base(options)
    {

    }
    
    
    
    public DbSet<Character> Characters { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Backpack> Backpacks { get; set; }
    public DbSet<Title> Titles { get; set; }

    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    
        modelBuilder.Entity<Character>().HasData(
            new Character { Id = 1, FirstName = "John", LastName = "Yakuza", CurrWei = 43, MaxWeight = 200 }
        );
    
        modelBuilder.Entity<Item>().HasData(
            new Item { Id = 1, Name = "Item1", Weight = 10 },
            new Item { Id = 2, Name = "Item2", Weight = 11 },
            new Item { Id = 3, Name = "Item3", Weight = 12 }
        );
    
        modelBuilder.Entity<Title>().HasData(
            new Title { Id = 1, Name = "Title1" },
            new Title { Id = 2, Name = "Title2" },
            new Title { Id = 3, Name = "Title3" }
        );
    
        modelBuilder.Entity<Backpack>().HasData(
            new Backpack { CharId = 1, ItemId = 1, Amount = 2 }
            // new Backpack { CharId = 1, ItemId = 2, Amount = 1 }
        );
    
        modelBuilder.Entity<CharacterTitle>().HasData(
            new CharacterTitle { CharacterId = 1, TitleId = 1, AcquiredAt = DateTime.Parse("2024-06-10") },
            new CharacterTitle { CharacterId = 1, TitleId = 2, AcquiredAt = DateTime.Parse("2024-06-09") },
            new CharacterTitle { CharacterId = 1, TitleId = 3, AcquiredAt = DateTime.Parse("2024-06-08") }
        );
    }

}