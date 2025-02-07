using DAL.Entities;

namespace Services.Dtos;

public class UpdatingUserDto
{
    public string? Login { get; set; }
    public string? Password { get; set; }
    public UserRole? Role { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Address { get; set; }
    public decimal? Discount { get; set; }
}