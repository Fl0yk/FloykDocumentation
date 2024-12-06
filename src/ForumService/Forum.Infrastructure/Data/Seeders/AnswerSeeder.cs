using Forum.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forum.Infrastructure.Data.Seeders;
public static class AnswerSeeder
{
    public static void SeedAnswer(this EntityTypeBuilder<Answer> builder)
    {
        builder.HasData([
            new Answer()
            {
                Id = Guid.Parse("a6c0936a-9d91-4d6a-b893-b257d5b255ca"),
                QuestionId = Guid.Parse("15F010ED-8C38-4EEB-B9EC-5FB56CCF3189"),
                ParentId = null,
                AuthorId = Guid.Parse("a6c0936a-9d91-4d6a-b893-b257d5b255c7"),
                Text = "Text 1",
                TimeOfCreation = DateTime.Now.AddDays(-3),
                Level = 0,
            },
            new Answer()
            {
                Id = Guid.Parse("5963ab4c-cb6f-4053-974e-2bd3da76ff6c"),
                QuestionId = Guid.Parse("15F010ED-8C38-4EEB-B9EC-5FB56CCF3189"),
                ParentId = Guid.Parse("a6c0936a-9d91-4d6a-b893-b257d5b255ca"),
                AuthorId = Guid.Parse("a6c0936a-9d91-4d6a-b893-b257d5b255c8"),
                Text = "Text 1-1",
                TimeOfCreation = DateTime.Now.AddDays(-2),
                Level = 1,
            },
            new Answer()
            {
                Id = Guid.Parse("31ed4f16-6f88-42f4-bc9e-ede50898b39a"),
                QuestionId = Guid.Parse("15F010ED-8C38-4EEB-B9EC-5FB56CCF3189"),
                ParentId = Guid.Parse("5963ab4c-cb6f-4053-974e-2bd3da76ff6c"),
                AuthorId = Guid.Parse("a6c0936a-9d91-4d6a-b893-b257d5b255c9"),
                Text = "Text 1-1-1",
                TimeOfCreation = DateTime.Now.AddDays(-1),
                Level = 2,
            },
            new Answer()
            {
                Id = Guid.Parse("2a621bcb-01ae-4803-8584-fe0542fdee5c"),
                QuestionId = Guid.Parse("15F010ED-8C38-4EEB-B9EC-5FB56CCF3189"),
                ParentId = Guid.Parse("a6c0936a-9d91-4d6a-b893-b257d5b255ca"),
                AuthorId = Guid.Parse("a6c0936a-9d91-4d6a-b893-b257d5b255c1"),
                Text = "Text 1-2",
                TimeOfCreation = DateTime.Now.AddDays(-3),
                Level = 1,
            },
            new Answer()
            {
                Id = Guid.Parse("126719aa-4937-484f-a1ce-0151730e4457"),
                QuestionId = Guid.Parse("45F010ED-8C38-4EEB-B9EC-5FB56CCF3189"),
                AuthorId = Guid.Parse("a6c0936a-9d91-4d6a-b893-b257d5b255c1"),
                ParentId = null,
                Text = "Text 2",
                TimeOfCreation = DateTime.Now.AddDays(-4),
                Level = 0,
            },
            ]);

    }
}
