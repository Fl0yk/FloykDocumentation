using Forum.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Infrastructure.Data.Seeders;
public static class QuestionSeeder
{
    public static void SeedQuestion(this EntityTypeBuilder<Question> builder)
    {
        builder.HasData([
            new Question(){
                Id = Guid.Parse("15F010ED-8C38-4EEB-B9EC-5FB56CCF3189"),
                AuthorId = Guid.Parse("24F010ED-8C38-4EEB-B9EC-5FB56CCF3189"),
                Title  = "Title 1",
                Description = "Description 1",
                DateOfCreation = DateTime.UtcNow.AddDays(-10)
            },
            new Question(){
                Id = Guid.Parse("25F010ED-8C38-4EEB-B9EC-5FB56CCF3189"),
                AuthorId = Guid.Parse("25F010ED-8C38-4EEB-B9EC-5FB56CCF3189"),
                Title  = "Title 2",
                Description = "Description 2",
                DateOfCreation = DateTime.UtcNow.AddMonths(-1)
            },
            new Question(){
                Id = Guid.Parse("35F010ED-8C38-4EEB-B9EC-5FB56CCF3189"),
                AuthorId = Guid.Parse("26F010ED-8C38-4EEB-B9EC-5FB56CCF3189"),
                Title  = "Title 3",
                Description = "Description 3",
                DateOfCreation = DateTime.UtcNow.AddDays(-1)
            },
            new Question(){
                Id = Guid.Parse("45F010ED-8C38-4EEB-B9EC-5FB56CCF3189"),
                AuthorId = Guid.Parse("26F010ED-8C38-4EEB-B9EC-5FB56CCF3189"),
                Title  = "Title 4",
                Description = "Description 4",
                DateOfCreation = DateTime.UtcNow.AddDays(-5)
            },
            ]);
    }
}
