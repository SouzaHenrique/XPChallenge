namespace XPChallenge.Infrastructure.Persistance;
public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepoitory
{
    public CustomerRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
    {
    }
}
