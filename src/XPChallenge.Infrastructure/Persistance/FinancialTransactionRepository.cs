namespace XPChallenge.Infrastructure.Persistance;
public class FinancialTransactionRepository : RepositoryBase<FinancialTransaction>, IFinancialTransactionRepository
{
    public FinancialTransactionRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
    {
    }
}
