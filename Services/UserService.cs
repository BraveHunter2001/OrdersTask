using DAL;
using DAL.Entities;
using DAL.Sequenser;
using Services.Dtos;

namespace Services;

public interface IUserService
{
    User CreateUser(UserDto model);
    Customer CreateCustomer(UserDto model);
    User? GetUserByLogin(string login);
    User? GetUserById(Guid id);
    void DeleteUser(User user);
    void UpdateUser(User origin, UpdatingUserDto model);
    void UpdateCustomer(Customer customer, UpdatingUserDto model);
}

internal class UserService(IUnitOfWork uow) : IUserService
{
    public User CreateUser(UserDto model)
    {
        User user = model.ToUser();

        uow.UserRepository.Insert(user);
        uow.Save();
        return user;
    }

    public Customer CreateCustomer(UserDto model)
    {
        Customer customer = model.ToCustomer();

        int counter = uow.GetNextValueSequence<int>(SequenceType.CustomerSequence);
        customer.Code = $"{counter:d4}-{DateTime.Now.Year}";

        uow.CustomerRepository.Insert(customer);
        uow.Save();

        return customer;
    }

    public User? GetUserByLogin(string login) => uow.UserRepository.GetUserByLogin(login);
    public User? GetUserById(Guid id) => uow.UserRepository.GetById(id);

    public void DeleteUser(User user)
    {
        uow.UserRepository.Delete(user);
        uow.Save();
    }

    public void UpdateUser(User origin, UpdatingUserDto model)
    {
        UpdateUserFields(origin, model);

        uow.UserRepository.Update(origin);
        uow.Save();
    }

    public void UpdateCustomer(Customer customer, UpdatingUserDto model)
    {
        UpdateUserFields(customer, model);

        if (!string.IsNullOrWhiteSpace(model.Name))
            customer.Name = model.Name;

        if (!string.IsNullOrWhiteSpace(model.Address))
            customer.Address = model.Address;

        if (model.Discount.HasValue)
            customer.Discount = model.Discount.Value;

        if (!string.IsNullOrWhiteSpace(model.Code))
            customer.Code = model.Code;

        uow.CustomerRepository.Update(customer);
        uow.Save();
    }

    private void UpdateUserFields(User origin, UpdatingUserDto model)
    {
        if (!string.IsNullOrWhiteSpace(model.Login))
            origin.Login = model.Login;

        if (!string.IsNullOrWhiteSpace(model.Password))
            origin.Password = model.Password;

        if (model.Role.HasValue)
            origin.Role = model.Role.Value;
    }
}