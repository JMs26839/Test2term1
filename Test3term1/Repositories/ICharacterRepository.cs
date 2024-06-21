using Microsoft.EntityFrameworkCore;
using Test3term1.Context;
using Test3term1.Models;

namespace Test3term1.Repositories;
    public interface ICharacterRepository
    {
        Task<Character> GetCharacterByIdAsync(int id);
        Task AddItemsToCharacterAsync(int characterId, List<Backpack> backpacks);
        Task<bool> SaveChangesAsync();
    }

    public class CharacterRepository : ICharacterRepository
    {
        private readonly APBDContext _context;

        public CharacterRepository(APBDContext context)
        {
            _context = context;
        }

        public async Task<Character> GetCharacterByIdAsync(int id)
        {
            return await _context.Characters
                .Include(c => c.ItemsInBackpacks)
                .ThenInclude(b => b.Item)
                .Include(c => c.Titles)
                .ThenInclude(ct => ct.Title)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddItemsToCharacterAsync(int characterId, List<Backpack> backpacks)
        {
            var character = await _context.Characters
                .Include(c => c.ItemsInBackpacks)
                .FirstOrDefaultAsync(c => c.Id == characterId);

            if (character == null)
                throw new ArgumentException("Character not found");

            int newWeight = backpacks.Sum(b => b.Item.Weight * b.Amount);
            if (character.CurrWei + newWeight > character.MaxWeight)
                throw new InvalidOperationException("Not enough capacity in backpack");

            foreach (var backpack in backpacks)
            {
                var existingBackpack = character.ItemsInBackpacks.FirstOrDefault(b => b.ItemId == backpack.ItemId);
                if (existingBackpack != null)
                {
                    existingBackpack.Amount += backpack.Amount;
                }
                else
                {
                    character.ItemsInBackpacks.Add(backpack);
                }
            }

            character.CurrWei += newWeight;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }

