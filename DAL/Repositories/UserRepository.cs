using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public interface IUserRepository : IRepository<User>
{
    User? GetUserByLogin(string login, bool readOnly = true);
}

internal class UserRepository(OrdersTaskContext context) : Repository<User>(context), IUserRepository
{
    public User? GetUserByLogin(string login, bool readOnly = true)
        => (readOnly ? context.Users.AsNoTracking() : context.Users)
            .FirstOrDefault(x => x.Login == login);
}