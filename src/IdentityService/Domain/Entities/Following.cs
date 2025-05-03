using Core.Abstractions;

namespace Identity.Domain.Entities;
public class Following : BaseEntity
{
    public Guid AuthorId {  get; set; }

    public User? Author { get; set; }

    public Guid UserId { get; set; }

    public User? User { get; set; }
}
