using MongoDB.Driver.Linq;
using XPChallenge.Domain.Commom.Models;

namespace XPChallenge.Infrastructure.Persistance;
public class RepositoryBase<T>(IMongoDatabase mongoDatabase) : IGenericRepository<T> where T : DomainEntity
{
    internal IMongoCollection<T> _collection => mongoDatabase.GetCollection<T>(typeof(T).Name);

    public Task<T> Add(T entity, CancellationToken cancellationToken)
    {
        entity.SetCreationDate(DateTime.UtcNow);
        _collection.InsertOneAsync(entity, null, cancellationToken);
        return Task.FromResult(entity);
    }

    public Task Delete(Guid entityId, CancellationToken cancellationToken)
    {
        var filter = Builders<T>.Filter.Eq(x => x.Id, entityId);
        return _collection.DeleteOneAsync(filter, cancellationToken);
    }

    public Task<IQueryable<T>> GetAll(CancellationToken cancellationToken)
    {
        IQueryable<T> result = _collection.AsQueryable();
        return Task.FromResult(result);
    }

    public async Task<T> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await _collection.AsQueryable().Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public Task<IQueryable<T>> GetPagedReponse(int page, int size, CancellationToken cancellationToken)
    {
        IQueryable<T> result = _collection.AsQueryable().Skip((page - 1) * size).Take(size);
        return Task.FromResult(result);
    }

    public Task Update(T entity, CancellationToken cancellationToken)
    {
        var filter = Builders<T>.Filter.Eq(x => x.Id, entity.Id);
        entity.SetUpdateDate(DateTime.Now);
        return _collection.ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken);
    }
}
