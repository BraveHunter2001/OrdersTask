using DAL.Entities;

namespace WebApi.ViewModel;

public class BaseUserViewModel
{
    public Guid UserId { get; set; }
    public string Login { get; set; }
    public UserRole Role { get; set; }

    public string Name { get; set; }
    public string Code { get; set; }
    public string Address { get; set; }
    public decimal? Discount { get; set; }

    public BaseUserViewModel(User user)
    {
        UserId = user.Id;
        Login = user.Login;
        Role = user.Role;

        if (user.Customer is null) return;
        Name = user.Customer.Name;
        Code = user.Customer.Code;
        Address = user.Customer.Address;
        Discount = user.Customer.Discount;
    }
}

public class UserViewModel(User user) : BaseUserViewModel(user)
{
    public string Password { get; set; } = user.Password;
}