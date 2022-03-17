using TheWorkBook.Backend.API.Middleware;

namespace TheWorkBook.Backend.API.Extensions
{
    public static class SwaggerAuthorizeExtensions
    {
        public static IApplicationBuilder UseSwaggerAuthorized(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SwaggerBasicAuthMiddleware>();
        }
    }
}
