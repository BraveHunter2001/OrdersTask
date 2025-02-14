using DAL.Dto;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public interface IUserRepository : IRepository<User>
{
    User? GetUserByLogin(string login, bool readOnly = true);
    PaginatedContainer<List<User>> GetPaginatedUserList(UserListFilter filter);
    User? GetUserById(Guid id, bool readOnly = true);
}

internal class UserRepository(OrdersTaskContext context) : Repository<User>(context), IUserRepository
{
    public User? GetUserByLogin(string login, bool readOnly = true)
        => (readOnly ? context.Users.AsNoTracking() : context.Users)
            .FirstOrDefault(x => x.Login == login);

    public PaginatedContainer<List<User>> GetPaginatedUserList(UserListFilter filter)
    {
        var query = context.Users
            .Include(u => u.Customer)
            .AsNoTracking()
            .Where(u =>
                (string.IsNullOrWhiteSpace(filter.Login) || u.Login.StartsWith(filter.Login))
                && (!filter.Role.HasValue || u.Role == filter.Role)
                && (string.IsNullOrWhiteSpace(filter.Code) || u.Customer.Code.StartsWith(filter.Code))
                && (string.IsNullOrWhiteSpace(filter.Address) || u.Customer.Address.StartsWith(filter.Address))
            )
            .OrderBy(u => u.Login);

        var result = GetPaginatedListContainer(query, filter);
        return result;
    }

    public User? GetUserById(Guid id, bool readOnly = true)
    {
        var query = context.Users
            .Include(u => u.Customer);

        if (readOnly)
            query.AsNoTracking();

        return query.FirstOrDefault(u => u.Id == id);
    }
}