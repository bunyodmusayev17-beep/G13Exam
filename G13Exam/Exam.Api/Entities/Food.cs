namespace Exam.Api.Entities;

public class Food
{
    public long FoodId { get; set; }
    public string name { get; set; }
    public string Discription { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }

  
    // Navigation prop
    public ICollection<Category> Categories { get; set; }
    public long CategoryId { get; set; }
}
