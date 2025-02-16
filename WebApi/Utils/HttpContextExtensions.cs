using DAL.Entities;
using Services;
using System.Security.Claims;

namespace WebApi.Utils;

public static class HttpContextExtensions
{
    public static Guid GetAuthorizedUserId(this HttpContext context) =>
        Guid.Parse(context.User.FindFirst("user_id")!.Value);

    public static UserRole GetAuthorizedUserRole(this HttpContext context) =>
        Enum.Parse<UserRole>(context.User.FindFirst(ClaimTypes.Role)!.Value);

    public static User GetAuthorizedUser(this HttpContext context, bool readOnly = true, bool includeCard = false)
    {
        var userService = context.RequestServices.GetService<IUserService>();

        var userId = context.GetAuthorizedUserId();
        User user = userService.GetUserById(userId, readOnly, includeCard)!;

        return user;
    }
}