using DAL.Entities;
using DAL.Repositories;
using DAL.Secuenser;

namespace DAL;

public interface IUnitOfWork
{
    IRepository<User> UserRepository { get; }
    IRepository<Customer> CustomerRepository { get; }

    ISequence<int> Sequence { get; }

    void Save();
}

internal class UnitOfWork(OrdersTaskContext context) : IUnitOfWork, IDisposable
{
    private IRepository<User> _userRepository;
    private IRepository<Customer> _customerRepository;

    private ISequence<int> _sequence;

    public IRepository<User> UserRepository => _userRepository ??= new Repository<User>(context);
    public IRepository<Customer> CustomerRepository => _customerRepository ??= new Repository<Customer>(context);

    public ISequence<int> Sequence => _sequence ??= new Sequence<int>(context);

    public void Save() => context.SaveChanges();

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (disposed) return;

        if (disposing)
        {
            context.Dispose();
        }

        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}