namespace Exam.Api.Dtos
{
    public class CategoryUpdateDto
    {
        public long CategoryId { get; set; }
        public string Name { get; set; } = null!;
    }
}
