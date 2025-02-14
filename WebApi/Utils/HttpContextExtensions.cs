using DAL.Entities;
using System.Security.Claims;

namespace WebApi.Utils;

public static class HttpContextExtensions
{
    public static Guid GetAuthorizedUserId(this HttpContext context) =>
        Guid.Parse(context.User.FindFirst("user_id")!.Value);

    public static UserRole GetAuthorizedUserRole(this HttpContext context) =>
        Enum.Parse<UserRole>(context.User.FindFirst(ClaimTypes.Role)!.Value);
}