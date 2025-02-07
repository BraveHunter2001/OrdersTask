namespace DAL.Repositories;

public interface IRepository<TEntity> : IDisposable
{
    TEntity? GetById(object id, bool readOnly = true);

    void Insert(TEntity entity);
    void InsertRange(IEnumerable<TEntity> entity);

    void Delete(object id);
    void Delete(TEntity entityToDelete);

    void Update(TEntity entityToUpdate);
}