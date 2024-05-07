namespace XPChallenge.Application.Contracts.Infrastructure.Persistance;
public interface IGenericRepository<T> where T : class
{
    Task<T> GetById(Guid id, CancellationToken cancellationToken);
    Task<IQueryable<T>> GetAll(CancellationToken cancellationToken);
    Task<IQueryable<T>> GetPagedReponse(int page, int size, CancellationToken cancellationToken);
    Task<T> Add(T entity, CancellationToken cancellationToken);
    Task Update(T entity, CancellationToken cancellationToken);
    Task Delete(Guid entityId, CancellationToken cancellationToken);
}
