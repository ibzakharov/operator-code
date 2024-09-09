namespace OperatorCode.Api.Middlewares;

public class RedirectToSwaggerMiddleware
{
    private readonly RequestDelegate _next;

    public RedirectToSwaggerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.Equals("/"))
        {
            context.Response.Redirect("/swagger", permanent: true);
            return;
        }

        await _next(context);
    }
}