namespace DAL.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
}

public enum UserRole
{
    Manager,
    Customer
}