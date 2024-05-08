using XPChallenge.Domain.Entities;

namespace XPChallenge.Application.Contracts.Infrastructure.Persistance;
public interface IFinancialProductRepository : IGenericRepository<FinancialProduct>
{
    Task<IEnumerable<FinancialProduct>> GetProductsByDueDate(DateTime dueDate);
}
