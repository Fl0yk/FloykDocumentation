using Article.Infrastructure.Shared.Models;
using MongoDB.Driver;

namespace Article.Infrastructure.Data.Seeders;

public static class CategoriesSeeder
{
    public static void SeedCategories(this IMongoCollection<CategoryDb> categories)
    {
        if (categories.EstimatedDocumentCount() != 0)
        {
            return;
        }

        categories.InsertMany([
            new CategoryDb()
            {
                Id = Guid.Parse("21db0e42-32ba-4a0b-a126-620aad2b1091"),
                Name = "Base C#",
                ParentId = null,
                Level = 0
            },
            new CategoryDb()
            {
                Id = Guid.Parse("af65248a-df56-456a-b84d-d1756ce06765"),
                Name = "C# types and functions",
                ParentId = Guid.Parse("21db0e42-32ba-4a0b-a126-620aad2b1091"),
                Level = 1
            },
            new CategoryDb()
            {
                Id = Guid.Parse("8eb020a2-7f4f-4726-8b3b-b3614a474ec7"),
                Name = "Classes, structures and namespaces",
                ParentId = Guid.Parse("21db0e42-32ba-4a0b-a126-620aad2b1091"),
                Level = 1
            },
            new CategoryDb()
            {
                Id = Guid.Parse("8f140628-270e-48fd-b206-92610afb6510"),
                Name = "Platform .NET",
                ParentId = null,
                Level = 0
            },
        ]);
    }
}
