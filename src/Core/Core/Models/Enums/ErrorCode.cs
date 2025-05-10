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

//Переписать ArticleService(несколбко бд(реляционка под юзера и категории, монго под статьи)+, транзакции, работа с юзером+, сохраненные статьи)
//Вынести ICurrentuserProvider в  Core, добавить работу с токеном во все сервисы+
//Пересмотреть хэндлеры в ApiGateway(нужны ли вообще они, если каждый сервис сам будет доставать данные из запроса)+
//Синхронизация юзеров+

//Статусы статьи? Типа, если вносят изменения в опубликованную статью, то она снимается с публикации? Или не давать изменять опубликованную
//В идеале сделать так: блоки хранят, опубликованы ли они. Пользователь добавляет блок, основная статья исходной версии, автор видит с новыми блоками

//ApproveArtcileCommand (Article; Admin)
//SaveArticleCommand (Article; Authorize)
//UnsaveArtileCommand (Article; Authorize)
//GetSavedArticlesByUser (Article; Authorize)
