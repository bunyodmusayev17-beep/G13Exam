using Exam.Api.Data;
using Exam.Api.Entities;

namespace Exam.Api.Repositories;

public class FoodRepository : BaseRepository<Food>, IFoodRepository
{
    public FoodRepository(AppDbContext dbContext)
        : base(dbContext)
    {
    }
}