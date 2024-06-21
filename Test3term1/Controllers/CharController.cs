using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test3term1.Context;
using Test3term1.DTO;
using Test3term1.Models;

namespace Test3term1.Controllers;

[Route("api[controller]")]
[ApiController]
public class CharController:ControllerBase
{
    private readonly APBDContext _apbdContext;




    public CharController(APBDContext apbdContext)
    {
        _apbdContext = apbdContext;
    }


     [HttpGet("{id}")]
    public async Task<ActionResult> GetCharacter(int id)
    {
        var character = await _apbdContext.Characters
            .Include(c => c.ItemsInBackpacks)
            .ThenInclude(b => b.Item)
            .Include(c => c.Titles)
            .ThenInclude(ct => ct.Title)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (character == null)
        {
            return NotFound();
        }

        var response = new
        {
            firstName = character.FirstName,
            lastName = character.LastName,
            currentWeight = character.CurrWei,
            maxWeight = character.MaxWeight,
            backpackItems = character.ItemsInBackpacks.Select(b => new
            {
                itemName = b.Item.Name,
                itemWeight = b.Item.Weight,
                amount = b.Amount
            }).ToList(),
            titles = character.Titles.Select(ct => new
            {
                title = ct.Title.Name,
                acquiredAt = ct.AcquiredAt
            }).ToList()
        };

        return Ok(response);
    }

    [HttpPost("{id}/backpacks")]
    public async Task<ActionResult> AddItemsToBackpack(int id, List<ItemDto> items)
    {
        var character = await _apbdContext.Characters
            .Include(c => c.ItemsInBackpacks)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (character == null)
        {
            return NotFound();
        }

        var itemIds = items.Select(i => i.ItemId).ToList();
        var dbItems = await _apbdContext.Items.Where(i => itemIds.Contains(i.Id)).ToListAsync();

        if (dbItems.Count != itemIds.Count)
        {
            return BadRequest("Some items do not exist in the database.");
        }

        var totalWeight = items.Sum(i => i.Amount * dbItems.First(dbi => dbi.Id == i.ItemId).Weight);
        if (character.CurrWei + totalWeight > character.MaxWeight)
        {
            return BadRequest("Not enough free weight to add all new items.");
        }

        foreach (var item in items)
        {
            var backpackItem = character.ItemsInBackpacks.FirstOrDefault(b => b.ItemId == item.ItemId);
            if (backpackItem != null)
            {
                backpackItem.Amount += item.Amount;
            }
            else
            {
                character.ItemsInBackpacks.Add(new Backpack
                {
                    CharId = character.Id,
                    ItemId = item.ItemId,
                    Amount = item.Amount
                });
            }
            character.CurrWei += item.Amount * dbItems.First(dbi => dbi.Id == item.ItemId).Weight;
        }

        await _apbdContext.SaveChangesAsync();

        var response = character.ItemsInBackpacks.Select(b => new
        {
            itemId = b.ItemId,
            characterId = b.CharId,
            amount = b.Amount
        }).ToList();

        return Ok(response);
    }
}
