namespace Web.Middleware
{
    public class UserRoleMiddleware
    {
        private readonly RequestDelegate _next;
        private const string RoleHeaderKey = "X-User-Role";

        public UserRoleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue(RoleHeaderKey, out var role))
            {
                context.Items["UserRole"] = role.ToString();
            }
            else
            {
                context.Items["UserRole"] = "user"; // Default role if header is missing
            }

            await _next(context);
        }
    }

    public static class UserRoleMiddlewareExtensions
    {
        public static IApplicationBuilder UseUserRole(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserRoleMiddleware>();
        }
    }
}
