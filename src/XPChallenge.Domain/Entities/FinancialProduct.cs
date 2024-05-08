using XPChallenge.Domain.Commom.Models;

namespace XPChallenge.Domain.Entities;
public class FinancialProduct : DomainEntity
{
    public string Name { get; private set; }
    public double CurrentPurchasePrice { get; private set; }
    public DateTime DueDate { get; private set; }

    public FinancialProduct(string name, double currentValue, DateTime dueDate)
    {
        Name = name;
        CurrentPurchasePrice = currentValue;
        DueDate = dueDate;
    }

    [BsonConstructor]
    public FinancialProduct(Guid id, string name, double purchasePrice, DateTime dueDate)
    {
        Id = id;
        Name = name;
        CurrentPurchasePrice = purchasePrice;
        DueDate = dueDate;
    }

    public void UpdateCurrentValue(double currentValue)
    {
        CurrentPurchasePrice = currentValue;
    }

    public void UpdateDueDate(DateTime dueDate)
    {
        DueDate = dueDate;
    }

    public void UpdateName(string name)
    {
        Name = name;
    }

}
