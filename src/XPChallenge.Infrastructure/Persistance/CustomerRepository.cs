namespace XPChallenge.Infrastructure.Persistance;
public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
{
    public CustomerRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
    {
    }
}
