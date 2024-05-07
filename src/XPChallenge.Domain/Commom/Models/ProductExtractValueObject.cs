namespace XPChallenge.Domain.Commom.Models;
public class ProductExtractValueObject
{
    public string Name { get; set; }

    public decimal? CurrentPrice { get; set; }

    public int? TotalQuantityPurchased { get; set; }

    public int? TotalQuantitySold { get; set; }

    public decimal? TotalAmountPurchased { get; set; }

    public decimal? TotalAmountSold { get; set; }

    public ProductExtractValueObject(int totalQuantityPurchased,
                                    int totalQuantitySold, decimal totalAmountPurchased,
                                    decimal totalAmountSold)
    {
        TotalQuantityPurchased = totalQuantityPurchased;
        TotalQuantitySold = totalQuantitySold;
        TotalAmountPurchased = totalAmountPurchased;
        TotalAmountSold = totalAmountSold;
    }

    public ProductExtractValueObject()
    {
        
    }
}
