namespace XPChallenge.Infrastructure.Persistance;
public class FinancialProductRepository : RepositoryBase<FinancialProduct>, IFinancialProductRepository
{
    public FinancialProductRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
    {
    }
}
