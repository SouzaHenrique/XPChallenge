using MongoDB.Driver.Linq;

namespace XPChallenge.Infrastructure.Persistance;
public class FinancialProductRepository : RepositoryBase<FinancialProduct>, IFinancialProductRepository
{
    public FinancialProductRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
    {
    }

    public async Task<IEnumerable<FinancialProduct>> GetProductsByDueDate(DateTime dueDate)
    {
        return await _collection.AsQueryable().Where(x => x.DueDate.Date <= dueDate.Date).ToListAsync();
    }
}
