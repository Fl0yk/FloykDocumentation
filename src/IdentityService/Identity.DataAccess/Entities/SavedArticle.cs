namespace Identity.DataAccess.Entities;
public class SavedArticle
{
    public Guid UserId {  get; set; }

    public User? User { get; set; }

    public Guid ArticleId { get; set; }
}
