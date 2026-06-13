
namespace Exam.Api.Repositories;

public interface IBaseRepository<T>
{
    IQueryable<T> GetAllQuery();
    Task AddAsync(T t);
    void Update(T t);
    void Delete(T t);
    Task SaveChangesAsync();
}