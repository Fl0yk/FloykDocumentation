namespace Identity.DataAccess.Entities;
public class Following
{
    public Guid AuthorId {  get; set; }

    public User? Author { get; set; }

    public Guid UserId { get; set; }

    public DateTime DateOfFollow { get; set; }
}
