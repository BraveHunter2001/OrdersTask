using DAL.Entities;

namespace Services.Dtos;

public class UserDto
{
    public required string Login { get; set; }
    public required string Password { get; set; }
    public UserRole Role { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public decimal? Discount { get; set; }

    public bool IsCustomer => Role == UserRole.Customer;

    public User ToUser() => new(Login, Password, Role);
    public Customer ToCustomer() => new(Login, Password, Role, Name, Address, Discount);
}