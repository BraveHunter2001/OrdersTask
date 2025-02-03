using DAL;
using DAL.Entities;
using DAL.Secuenser;
using Services.Dtos;

namespace Services;

public interface IUserService
{
    User CreateUser(CreationUserDto model);
    User? GetUserByLogin(string login);
}

internal class UserService(IUnitOfWork uow) : IUserService
{
    public User CreateUser(CreationUserDto model)
    {
        bool isCustomer = model.Role == UserRole.Customer;
        User user = isCustomer ? model.ToCustomer() : model.ToUser();
        if (isCustomer)
            CreateCustomer(user as Customer);
        else
        {
            uow.UserRepository.Insert(user);
            uow.Save();
        }

        return user;
    }

    public User? GetUserByLogin(string login) => uow.UserRepository.GetUserByLogin(login);

    private void CreateCustomer(Customer customer)
    {
        int counter = uow.Sequence.GetNextValue(SequenceType.CustomerSequence);
        customer.Code = $"{counter:d4}-{DateTime.Now.Year}";

        uow.CustomerRepository.Insert(customer);
        uow.Save();
    }
}