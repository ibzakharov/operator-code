namespace OperatorCode.Api.Middlewares.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseRedirectToSwagger(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RedirectToSwaggerMiddleware>();
    }
}