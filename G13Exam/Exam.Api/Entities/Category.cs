namespace Exam.Api.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Food> Foods { get; set; } = new List<Food>();
    }
}
