using Exam.Api.Data;
using Exam.Api.Entities;

namespace Exam.Api.Repositories;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext dbContext)
        : base(dbContext)
    {
    }
}