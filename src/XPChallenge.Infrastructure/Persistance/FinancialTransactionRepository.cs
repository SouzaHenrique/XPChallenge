using Microsoft.VisualBasic;
using MongoDB.Driver.Linq;
using XPChallenge.Domain.Commom.Models;

namespace XPChallenge.Infrastructure.Persistance;
public class FinancialTransactionRepository : RepositoryBase<FinancialTransaction>, IFinancialTransactionRepository
{
    public FinancialTransactionRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
    {
    }

    public IQueryable<FinancialTransaction> GetByProductId(Guid productId, CancellationToken cancellationToken)
    {
        return _collection.AsQueryable().Where(x => x.FinancialProductID == productId);
    }

    public ProductExtractValueObject GetProductExtractByProduct(FinancialProduct financialProduct, CancellationToken cancellationToken)
    {
        var productExtract = _collection.AsQueryable()
            .Where(transaction => transaction.FinancialProductID == financialProduct.Id)
            .GroupBy(transaction => new { transaction.FinancialProductID, transaction.TransactionType })
            .Select(group => new
            {
                TransactionType = group.Key.TransactionType,
                TotalQuantity = group.Sum(transaction => transaction.Quantity),
                TotalAmount = group.Sum(transaction => transaction.Quantity * transaction.UnitPrice)
            })
            .ToList(); // Execute query to avoid multiple executions of the same query

        var purchases = productExtract.FirstOrDefault(x => x.TransactionType == Domain.Enums.TransactionTypeEnum.Purchase);
        var sales = productExtract.FirstOrDefault(x => x.TransactionType == Domain.Enums.TransactionTypeEnum.Sale);

        return new ProductExtractValueObject
        {
            Name = financialProduct.Name,
            TotalQuantityPurchased = purchases?.TotalQuantity,
            TotalAmountPurchased = purchases?.TotalAmount,
            TotalQuantitySold = sales?.TotalQuantity,
            TotalAmountSold = sales?.TotalAmount,
            CurrentPrice = financialProduct.CurrentPurchasePrice,
            DueDate = financialProduct.DueDate
        };
    }
}
