namespace Article.Domain.Entities;

public static class BlockType
{
    public const string Title = "title";

    public const string Text = "text";

    public const string Code = "code";

    public static string[] Types = [Title, Text, Code];
}
//    public static BlockType Title => new(1, "title");

//    public static BlockType Text => new(2, "text");

//    public static BlockType Code => new BlockType(3, "code");

//    public int Id { get; init; }

//    public string Name { get; init; }

//    private BlockType(int id, string name)
//    {
//        Id = id;
//        Name = name;
//    }

//    public override string ToString() => Name;

//    public static explicit operator int(BlockType type) => type.Id;
//}
