using XPChallenge.Domain.Commom.Models;
namespace XPChallenge.Domain.Entities;
public class Customer : DomainEntity
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public decimal Balance { get; private set; }
    public IEnumerable<PurchasedProductValueObject> PurchasedProducts { get; private set; }

    public Customer(string firstName, string lastName, decimal balance, IEnumerable<PurchasedProductValueObject> purchasedProducts)
    {
        FirstName = firstName;
        LastName = lastName;
        Balance = balance;
        PurchasedProducts = purchasedProducts;
    }

    [BsonConstructor]
    public Customer(Guid id, string firstName, string lastName, decimal balance, IEnumerable<PurchasedProductValueObject> purchasedProducts)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Balance = balance;
        PurchasedProducts = purchasedProducts;
    }

    public void PurchaseFinancialProduct(FinancialProduct purchasedProduct, int quantity)
    {
        PurchasedProducts ??= [];

        var product = PurchasedProducts.FirstOrDefault(x => x.FinancialProductID == purchasedProduct.Id);

        if (product is null)
        {
            PurchasedProducts = PurchasedProducts.Append(new PurchasedProductValueObject(purchasedProduct, quantity));
            Balance -= (quantity * purchasedProduct.CurrentPurchasePrice);
        }
        else
        {
            product.Quantity += quantity;
            Balance -= (quantity * purchasedProduct.CurrentPurchasePrice);
        }
    }

    public void SellFinancialProduct(Guid financialProductID, int quantity, decimal pricePerUnit)
    {
        PurchasedProducts ??= [];

        var product = PurchasedProducts.FirstOrDefault(x => x.FinancialProductID == financialProductID);

        if (product is not null)
        {
            Balance += (quantity * pricePerUnit);
            product.Quantity -= quantity;

            if (product.Quantity <= 0)
            {
                PurchasedProducts = PurchasedProducts.Where(x => x.FinancialProductID != financialProductID);
            }
        }
    }

    public void IncreaseBalanceIn(decimal amount)
    {
        if (Balance <= 0)
        {
            return;
        }

        Balance += amount;
    }

    public void UpdateFirstName(string firstName)
    {
        if (string.IsNullOrEmpty(firstName))
        {
            return;
        }

        FirstName = firstName;
    }

    public void UpdateLastName(string lastName)
    {
        if (string.IsNullOrEmpty(lastName))
        {
            return;
        }

        LastName = lastName;
    }
}
