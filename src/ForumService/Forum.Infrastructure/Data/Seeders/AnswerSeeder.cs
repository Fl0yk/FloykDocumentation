using Forum.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Infrastructure.Data.Seeders;
public static class AnswerSeeder
{
    public static void SeedAnswer(this EntityTypeBuilder<Answer> builder)
    {
        builder.HasData([
            
            ]);
    }
}
