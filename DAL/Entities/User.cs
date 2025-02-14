namespace DAL.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
    public Customer? Customer { get; set; }

    public bool IsCustomer => Role == UserRole.Customer;

    public User()
    {
    }

    public User(string login, string password, UserRole role)
    {
        Login = login;
        Password = password;
        Role = role;
    }
}

public enum UserRole
{
    Manager,
    Customer
}