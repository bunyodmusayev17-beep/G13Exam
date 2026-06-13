using Exam.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exam.Api.FluentApies
{
    public class FoodConfiguration : IEntityTypeConfiguration<Food>
    {
        public void Configure(EntityTypeBuilder<Food> builder)
        {
            builder.ToTable("Foods");
            builder.HasKey(f => f.FoodId);
            builder.Property(f => f.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(f => f.Price)
                .HasPrecision(18, 2)
                .IsRequired();
            builder.Property(f => f.IsAvailable)
                .IsRequired();
            builder.HasOne(f => f.Category)
                .WithMany(c => c.Foods)
                .HasForeignKey(f => f.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
