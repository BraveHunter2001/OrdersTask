using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

internal class Repository<TEntity>(OrdersTaskContext context) : IRepository<TEntity>, IDisposable
    where TEntity : class
{
    protected DbSet<TEntity> dbSet = context.Set<TEntity>();

    private bool disposed = false;

    public virtual TEntity GetById(object id) => dbSet.Find(id);

    public virtual void Insert(TEntity entity) => dbSet.Add(entity);

    public void InsertRange(IEnumerable<TEntity> entity) => dbSet.AddRange(entity);

    public virtual void Delete(object id)
    {
        TEntity entityToDelete = dbSet.Find(id);
        Delete(entityToDelete);
    }

    public virtual void Delete(TEntity entityToDelete)
    {
        if (context.Entry(entityToDelete).State == EntityState.Detached)
        {
            dbSet.Attach(entityToDelete);
        }

        dbSet.Remove(entityToDelete);
    }

    public virtual void Update(TEntity entityToUpdate)
    {
        dbSet.Attach(entityToUpdate);
        context.Entry(entityToUpdate).State = EntityState.Modified;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                context.Dispose();
            }
        }

        this.disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}