namespace XPChallenge.Domain.Commom.Models;
public abstract class DomainEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public void SetCreationDate(DateTime createdAt)
    {
        CreatedAt = createdAt;
    }

    public void SetUpdateDate(DateTime updatedAt)
    {
        UpdatedAt = updatedAt;
    }
}
