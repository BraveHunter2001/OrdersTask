using DAL.Entities;

namespace DAL.Dto;

public class UserListFilter : PaginatedFilter
{
    public string? Login { get; set; }
    public UserRole? Role { get; set; }
    public string? Code { get; set; }
    public string? Address { get; set; }
}