using System.Net;
using System.Security.Claims;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace API.Middlewares;

public class EnsureUserExistsMiddleware
{
    private readonly RequestDelegate _next;

    public EnsureUserExistsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, UserManager<ApplicationUser> userManager)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId != null && !userManager.Users.Any(u => u.Id == userId))
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("User does not exist");
            return;
        }

        await _next(context);
    }
}