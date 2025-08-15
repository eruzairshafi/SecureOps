using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SecureOps.Authorize.Models;
using SecureOps.Services;
using SecureOps.Services.Cache.Options;
using System.Security.Claims;

namespace SecureOps.Authorize;

/// <summary>
/// Specifies that the user must have a specific permission to access the decorated resource.
/// </summary>
/// <remarks>This attribute is used to enforce authorization based on a required permission. It checks whether the
/// authenticated user has the specified permission by delegating to an <see cref="IPermissionService" />. If the user
/// is not authenticated, the request results in an <see cref="UnauthorizedResult" />. If the user lacks the required
/// permission, the request results in a <see cref="ForbidResult" />.</remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="HasPermissionAttribute"/> class with the specified permission.
/// </remarks>
/// <param name="permission">The name of the permission required to access the associated resource. Cannot be null or empty.</param>
/// <param name="description"></param>
/// <param name="category"></param>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
public class HasPermissionAttribute(string permission, string description, string category) : Attribute, IAsyncAuthorizationFilter
{
    internal readonly PermissionModel Permission = new(permission, description, category);

    /// <summary>
    /// Handles authorization for the current HTTP request by validating the user's identity and permissions.
    /// </summary>
    /// <remarks>This method checks whether the user is authenticated and has the required permissions to
    /// access the resource. If the user is not authenticated, the result is set to <see cref="UnauthorizedResult"/>. If
    /// the user lacks the required permissions, the result is set to <see cref="ForbidResult"/>.</remarks>
    /// <param name="context">The <see cref="AuthorizationFilterContext"/> containing the HTTP context and other information about the
    /// request.</param>
    /// <returns></returns>
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        ClaimsPrincipal user = context.HttpContext.User;

        SecureOpsOptions secureOpsOptions = context.HttpContext.RequestServices
            .GetRequiredService<SecureOpsOptions>();

        var userId = user.Claims.FirstOrDefault(c => c.Type == secureOpsOptions.UserIdClaimType)?.Value;

        if (!(user?.Identity?.IsAuthenticated ?? false))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (string.IsNullOrEmpty(userId))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var permissionService = context.HttpContext.RequestServices
            .GetRequiredService<IPermissionService>();

        bool hasPermission = await permissionService.HasPermissionAsync(userId, Permission.Name);

        if (!hasPermission)
        {
            context.Result = new ForbidResult();
        }
    }
}