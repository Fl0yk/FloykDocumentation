namespace Core.Models.Enums;

public enum ErrorCode
{
    App = 30000,
    Argument = 30001,
    NotFound = 30002,
    Unauthorized = 30003,
    Forbidden = 30004,
    //TODO: add already exists (identity)
    //TODO: add not author (article)
}

// Redis Jwt token? Napizdec?
// ArticleRepository: add filter isDeleted = false
// Article command: add transaction provider? Mongo transactions?

//Статусы статьи? Типа, если вносят изменения в опубликованную статью, то она снимается с публикации? Или не давать изменять опубликованную
//В идеале сделать так: блоки хранят, опубликованы ли они. Пользователь добавляет блок, основная статья исходной версии, автор видит с новыми блоками

