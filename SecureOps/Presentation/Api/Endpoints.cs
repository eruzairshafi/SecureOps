using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SecureOps.Presentation.Api.Options;
using SecureOps.Presentation.Endpoints.Model;
using SecureOps.Services;

namespace SecureOps.Presentation.Api;

internal class Endpoints
{
    private static SecureOpsEndpointsOptions options = new SecureOpsEndpointsOptions();

    /// <summary>
    /// Configures and maps permission-related endpoints to the specified <see cref="IEndpointRouteBuilder"/>.
    /// </summary>
    /// <remarks>This method maps endpoints for managing user and global permissions based on the
    /// configuration provided. The following endpoints may be mapped, depending on the options: <list type="bullet">
    /// <item> <description>User permission management endpoints, including adding, removing, and retrieving permissions
    /// for a specific user.</description> </item> <item> <description>Global permission management endpoints, including
    /// adding and removing global permissions.</description> </item> <item> <description>An endpoint for listing all
    /// permissions.</description> </item> </list> Authorization requirements can be applied to the endpoints based on
    /// the <see cref="SecureOpsEndpointsOptions.PermissionClaim"/>.</remarks>
    /// <param name="app">The <see cref="IEndpointRouteBuilder"/> used to define the endpoints.</param>
    /// <param name="configure">An optional delegate to configure <see cref="SecureOpsEndpointsOptions"/> for customizing endpoint behavior. If
    /// not provided, default options are used.</param>
    internal static void MapPermissionEndpoints(
        IEndpointRouteBuilder app,
        Action<SecureOpsEndpointsOptions>? configure = null)
    {
        configure?.Invoke(options);

        var group = app.MapGroup(options.RoutePrefix).WithTags("Permissions");

        if (!string.IsNullOrWhiteSpace(options.PermissionClaim))
        {
            group.RequireAuthorization(policy =>
                policy.RequireClaim(options.PermissionClaim));
        }

        if (options.EnableUserPermissionManagement)
        {
            group.MapPost("user/{userId}/add", async (
                IPermissionService service,
                string userId,
                PermissionRequest req) =>
            {
                await service.AddPermissionToUserAsync(userId, req.Permission);
                return Results.Ok();
            });

            group.MapPost("user/{userId}/remove", async (
                IPermissionService service,
                string userId,
                PermissionRequest req) =>
            {
                await service.RemovePermissionFromUserAsync(userId, req.Permission);
                return Results.Ok();
            });

            group.MapGet("user/{userId}", async (
                IPermissionService service,
                string userId) =>
            {
                var perms = await service.GetUserPermissionsAsync(userId);
                return Results.Ok(perms);
            });
        }

        // ✅ Global Permissions
        if (options.EnableGlobalPermissionManagement)
        {
            group.MapPost("global/add", async (
                IPermissionService service,
                PermissionRequest req) =>
            {
                await service.AddGlobalPermissionAsync(req.Permission);
                return Results.Ok();
            });

            group.MapPost("global/remove", async (
                IPermissionService service,
                PermissionRequest req) =>
            {
                await service.RemoveGlobalPermissionAsync(req.Permission);
                return Results.Ok();
            });
        }

        // ✅ List All
        if (options.EnableListingAllPermissions)
        {
            group.MapGet("all", async (IPermissionService service) =>
            {
                var all = await service.GetAllPermissionsAsync();
                return Results.Ok(all);
            });
        }

    }
}
