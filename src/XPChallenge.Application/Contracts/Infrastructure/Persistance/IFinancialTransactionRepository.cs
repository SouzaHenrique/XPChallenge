using XPChallenge.Domain.Commom.Models;
using XPChallenge.Domain.Entities;

namespace XPChallenge.Application.Contracts.Infrastructure.Persistance;
public interface IFinancialTransactionRepository : IGenericRepository<FinancialTransaction>
{
    IQueryable<FinancialTransaction> GetByProductId(Guid productId, CancellationToken cancellationToken);
    ProductExtractValueObject GetProductExtractByProduct(FinancialProduct financialProduct, CancellationToken cancellationToken);
}
