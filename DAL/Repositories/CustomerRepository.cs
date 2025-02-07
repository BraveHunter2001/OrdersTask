using DAL.Entities;

namespace DAL.Repositories;

public interface ICustomerRepository : IRepository<Customer>
{
    bool HasCustomerByCode(string code);
}

internal class CustomerRepository(OrdersTaskContext context) : Repository<Customer>(context), ICustomerRepository
{
    public bool HasCustomerByCode(string code) => context.Customers.Any(x => x.Code == code);
}