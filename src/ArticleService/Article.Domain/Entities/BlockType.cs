namespace Article.Domain.Entities;

//TODO: ничего себе... Переписать на енам
public static class BlockType
{
    public const string Title = "title";

    public const string Text = "text";

    public const string Code = "code";

    public static string[] Types = [Title, Text, Code];
}
