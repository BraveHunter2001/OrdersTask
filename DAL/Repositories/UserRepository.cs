using DAL.Entities;

namespace DAL.Repositories;

public interface IUserRepository : IRepository<User>
{
    User? GetUserByLogin(string login);
}

internal class UserRepository(OrdersTaskContext context) : Repository<User>(context), IUserRepository
{
    public User? GetUserByLogin(string login) => context.Users.FirstOrDefault(x => x.Login == login);
}