﻿using DAL.Repositories;
using DAL.Sequenser;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    ICustomerRepository CustomerRepository { get; }
    IOrderRepository OrderRepository { get; }
    IItemRepository ItemRepository { get; }
    T GetNextValueSequence<T>(SequenceType sequenceType);

    void Save();
}

internal class UnitOfWork(OrdersTaskContext context) : IUnitOfWork, IDisposable
{
    private IUserRepository _userRepository;
    private ICustomerRepository _customerRepository;
    private IItemRepository _itemRepository;
    private IOrderRepository _orderRepository;

    public IUserRepository UserRepository => _userRepository ??= new UserRepository(context);
    public ICustomerRepository CustomerRepository => _customerRepository ??= new CustomerRepository(context);
    public IItemRepository ItemRepository => _itemRepository ??= new ItemRepository(context);
    public IOrderRepository OrderRepository => _orderRepository ??= new OrderRepository(context);

    public T GetNextValueSequence<T>(SequenceType sequenceType)
    {
        string sql = $"select nextval('\"{sequenceType.ToString()}\"')";
        return context.Database.SqlQueryRaw<T>(sql).AsEnumerable().First();
    }

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