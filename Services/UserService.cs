using DAL;
using DAL.Dto;
using DAL.Entities;
using DAL.Sequenser;
using Services.Dtos;

namespace Services;

public interface IUserService
{
    User CreateUser(UserDto model);
    User? GetUserByLogin(string login);
    User? GetUserById(Guid id, bool readOnly = true);
    void DeleteUser(User user);
    void UpdateUser(User origin, UpdatingUserDto model);
    PaginatedContainer<List<User>> GetPaginatedUserList(UserListFilter filter);
}

internal class UserService(IUnitOfWork uow) : IUserService
{
    public User CreateUser(UserDto model)
    {
        User user = model.ToUser();

        if (model.IsCustomer)
            CreateCustomer(user, model.Name, model.Address, model.Discount);

        uow.UserRepository.Insert(user);
        uow.Save();
        return user;
    }

    private void CreateCustomer(User user, string name, string address, decimal? discount)
    {
        Customer customer = new()
        {
            Name = name,
            Address = address,
            Discount = discount,
            Code = GenerateCode(),
        };

       user.Customer = customer;
    }

    private string GenerateCode()
    {
        int counter = uow.GetNextValueSequence<int>(SequenceType.CustomerSequence);
        return $"{counter:d4}-{DateTime.Now.Year}";
    }

    public User? GetUserByLogin(string login) => uow.UserRepository.GetUserByLogin(login);
    public User? GetUserById(Guid id, bool readOnly = true) => uow.UserRepository.GetUserById(id, readOnly);

    public void DeleteUser(User user)
    {
        uow.UserRepository.Delete(user);

        uow.Save();
    }

    public void UpdateUser(User origin, UpdatingUserDto model)
    {
        if (model.Role.HasValue && model.Role.Value != origin.Role)
            SwitchRole(origin, model);

        UpdateUserFields(origin, model);
        if (origin.IsCustomer)
            UpdateCustomerFields(origin.Customer!, model);

        uow.UserRepository.Update(origin);
        uow.Save();
    }

    private void SwitchRole(User origin, UpdatingUserDto model)
    {
        switch (origin.Role, model.Role)
        {
            case (UserRole.Customer, UserRole.Manager):
                var customer = origin.Customer;

                origin.Customer = null;
                uow.CustomerRepository.Delete(customer);
                break;

            case (UserRole.Manager, UserRole.Customer):
                CreateCustomer(origin, model.Name, model.Address, model.Discount);
                break;
        }
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

    private void UpdateCustomerFields(Customer customer, UpdatingUserDto model)
    {
        if (!string.IsNullOrWhiteSpace(model.Name))
            customer.Name = model.Name;

        if (!string.IsNullOrWhiteSpace(model.Address))
            customer.Address = model.Address;

        if (model.Discount.HasValue)
            customer.Discount = model.Discount.Value;

        if (!string.IsNullOrWhiteSpace(model.Code))
            customer.Code = model.Code;
        else if (string.IsNullOrWhiteSpace(customer.Code))
            customer.Code = GenerateCode();
    }

    public PaginatedContainer<List<User>> GetPaginatedUserList(UserListFilter filter) =>
        uow.UserRepository.GetPaginatedUserList(filter);
}