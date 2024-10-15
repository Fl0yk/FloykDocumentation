using Forum.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Infrastructure.Data.Seeders;
public static class QuestionSeeder
{
    public static void SeedQuestion(this EntityTypeBuilder<Question> builder)
    {
        builder.HasData([
            
            ]);
    }
}
