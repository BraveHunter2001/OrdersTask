using DAL.Entities;

namespace WebApi.ViewModel;

public class UserListItemViewModel(User user) : BaseUserViewModel(user)
{
    public string RoleName => Role.ToString();
}