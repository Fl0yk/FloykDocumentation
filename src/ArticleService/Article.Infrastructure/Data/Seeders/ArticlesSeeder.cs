using Article.Domain.Entities;
using Article.Infrastructure.Shared.Models;
using MongoDB.Driver;

namespace Article.Infrastructure.Data.Seeders;

public static class ArticlesSeeder
{
    public static void SeedArticles(this IMongoCollection<ArticleDb> articles)
    {
        if (articles.EstimatedDocumentCount() != 0)
        {
            return;
        }

        articles.InsertMany([
            new ArticleDb()
            {
                Id = Guid.Parse("810b1d52-8546-41a8-9d7c-31406ec364b8"),
                Title = "Numeric data types",
                AuthorName = "Author",
                IsPublished = true,
                DateOfPublication = DateTime.UtcNow.AddDays(-30),
                CategoryId = Guid.Parse("af65248a-df56-456a-b84d-d1756ce06765"),
                Blocks = [
                    new BlockDb()
                    {
                        Id = Guid.Parse("35c5f119-358b-4666-9930-20a041a9dbae"),
                        Text = "Numeric data types: int, long, float and etc.",
                        Type = BlockType.Title,
                    },
                    new BlockDb()
                    {
                        Id = Guid.Parse("eb3b7423-3439-4260-aa90-0dd2a1e7e5bf"),
                        Text = "Text about type int",
                        Type = BlockType.Text,
                    },
                    new BlockDb()
                    {
                        Id = Guid.Parse("8b94d1a7-f6ba-43fc-8a58-033e06d0e7bd"),
                        Text = "Text about type long",
                        Type = BlockType.Text,
                    },
                    new BlockDb()
                    {
                        Id = Guid.Parse("cea32b68-0be1-48e8-af5c-d20d4d728444"),
                        Text = "Text about type float",
                        Type = BlockType.Text,
                    }
                ]
            },
            new ArticleDb()
            {
                Id = Guid.Parse("499b4ad1-1e48-4c50-8b97-0f9465815246"),
                Title = "Difference between class and structure",
                AuthorName = "Author",
                IsPublished = false,
                DateOfPublication = null,
                CategoryId = Guid.Parse("8eb020a2-7f4f-4726-8b3b-b3614a474ec7"),
                Blocks = [
                    new BlockDb()
                    {
                        Id = Guid.Parse("b1db98db-109c-426b-8574-93132354eca8"),
                        Text = "Principle of operation and storage in memory",
                        Type = BlockType.Title,
                    },
                    new BlockDb()
                    {
                        Id = Guid.Parse("430bb883-b83e-41be-93bb-748a4e1ea6ba"),
                        Text = "How are they stored in memory...",
                        Type = BlockType.Text,
                    }
                ]
            },
            ]);
    }
}
