using Forum.Domain.Entities;
using Forum.Infrastructure.Data.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Infrastructure.Data.EntityConfigurations;
public class AnswerEntityTypeConfigurator : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Text).IsRequired();
        builder.Property(a => a.TimeOfCreation).IsRequired();

        builder.SeedAnswer();
    }
}
