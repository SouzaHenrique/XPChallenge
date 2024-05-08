namespace XPChallenge.Domain.Commom.Models;
public class ProductExtractValueObject
{
    public string Name { get; set; }

    public double? CurrentPrice { get; set; }

    public int? TotalQuantityPurchased { get; set; }

    public int? TotalQuantitySold { get; set; }

    public double? TotalAmountPurchased { get; set; }

    public double? TotalAmountSold { get; set; }
    public DateTime DueDate { get; set; }

    public ProductExtractValueObject(int totalQuantityPurchased,
                                    int totalQuantitySold, double totalAmountPurchased,
                                    double totalAmountSold)
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
