using XPChallenge.Domain.Entities;

namespace XPChallenge.Domain.Commom.Models;
public class PurchasedProductValueObject
{
    public PurchasedProductValueObject(FinancialProduct product, int quantity)
    {
        FinancialProductID = product.Id;
        Quantity = quantity;
    }
    public Guid FinancialProductID { get; set; }
    public int Quantity { get; set; }
}
