namespace Exam.Api.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly AppDbContext DbContext;

    public BaseRepository(AppDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task AddAsync(T t)
    {
        await DbContext.AddAsync(t);
    }

    public void Delete(T t)
    {
        DbContext.Remove(t);
    }

    public IQueryable<T> GetAllQuery()
    {
        return DbContext.Set<T>();
    }

    public async Task SaveChangesAsync()
    {
        await DbContext.SaveChangesAsync();
    }

    public void Update(T t)
    {
        DbContext.Update(t);
    }
}

