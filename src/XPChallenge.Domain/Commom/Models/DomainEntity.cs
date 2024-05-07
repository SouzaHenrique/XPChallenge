namespace XPChallenge.Domain.Commom.Models;
public abstract class DomainEntity
{
    public Guid Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }
}
