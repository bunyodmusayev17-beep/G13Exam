using Exam.Api.Data;
using Exam.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Exam.Api.Repositories;

public class FoodRepository : IFoodRepository
{


    private readonly AppDbContext _context;

    public FoodRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Food>> GetAllAsync()
    {
        return await _context.Foods
            .Include(f => f.Category)
            .ToListAsync();
    }

    public async Task<Food?> GetByIdAsync(long id)
    {
        return await _context.Foods
            .Include(f => f.Category)
            .FirstOrDefaultAsync(f => f.FoodId == id);
    }

    public async Task<Food> CreateAsync(Food food)
    {
        await _context.Foods.AddAsync(food);
        await _context.SaveChangesAsync();

        return food;
    }

    public async Task<Food> UpdateAsync(Food food)
    {
        _context.Foods.Update(food);
        await _context.SaveChangesAsync();

        return food;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var food = await _context.Foods
            .FirstOrDefaultAsync(f => f.FoodId == id);

        if (food is null)
            return false;

        _context.Foods.Remove(food);
        await _context.SaveChangesAsync();

        return true;
    }
}