namespace WebApi.Utils;

public static class HttpContextExtensions
{
    public static Guid GetAuthorizedUserId(this HttpContext context) =>
        Guid.TryParse(context.User.FindFirst("user_id")?.Value, out Guid id) ? id : default;
}