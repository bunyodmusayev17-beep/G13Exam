namespace Exam.Api.Dtos
{
    public class FoodUpdateDto
    {
        public long FoodId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public int CategoryId { get; set; }
    }
}
