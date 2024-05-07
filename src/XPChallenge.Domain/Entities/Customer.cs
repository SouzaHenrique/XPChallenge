using XPChallenge.Domain.Commom.Models;

namespace XPChallenge.Domain.Entities;
public class Customer : DomainEntity
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public decimal Balance { get; private set; }
    public IEnumerable<Guid> PurchasedProducts { get; private set; }

    public Customer(string firstName, string lastName, decimal balance, IEnumerable<Guid> purchasedProducts)
    {
        FirstName = firstName;
        LastName = LastName;
        Balance = balance;
        PurchasedProducts = purchasedProducts;
    }

    [BsonConstructor]
    public Customer(Guid id, string firstName, string lastName, decimal balance, IEnumerable<Guid> purchasedProducts)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Balance = balance;
        PurchasedProducts = purchasedProducts;
    }

    public void AddProduct(Guid productId)
    {
        PurchasedProducts = PurchasedProducts.Append(productId);
    }

    public void RemoveProduct(Guid productId)
    {
        PurchasedProducts = PurchasedProducts.Where(x => x != productId);
    }

    public void UpdateBalance(decimal amount)
    {
        Balance += amount;
    }

    public void UpdateFirstName(string firstName)
    {
        FirstName = firstName;
    }

    public void UpdateLastName(string lastName)
    {
        LastName = lastName;
    }
}
