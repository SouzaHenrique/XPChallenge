using XPChallenge.Domain.Commom.Models;

namespace XPChallenge.Domain.Entities;
public class FinancialTransaction : DomainEntity
{
    public Guid FinancialProductID { get; private set; }
    public Guid CustomerID { get; private set; }
    public TransactionTypeEnum  TransactionType { get; private set; }
    public int Quantity { get; private set; }
    public double UnitPrice { get; private set; }
    public DateTime TransactionDate { get; private set; }

    public FinancialTransaction(Guid financialProductId,
                                TransactionTypeEnum transactionType, int quantity,
                                double unitPrice, DateTime transactionDate, Guid customerID)
    {
        FinancialProductID = financialProductId;
        TransactionType = transactionType;
        Quantity = quantity;
        UnitPrice = unitPrice;
        TransactionDate = transactionDate;
        CustomerID = customerID;
    }

    [BsonConstructor]
    public FinancialTransaction(Guid id, Guid financialProductId,
                                TransactionTypeEnum transactionType, int quantity,
                                double unitPrice, DateTime transactionDate, Guid customerID)
    {
        Id = id;
        FinancialProductID = financialProductId;
        TransactionType = transactionType;
        Quantity = quantity;
        UnitPrice = unitPrice;
        TransactionDate = transactionDate;
        CustomerID = customerID;
    }
}
