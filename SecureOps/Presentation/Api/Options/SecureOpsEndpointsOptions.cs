using SecureOps.Options;

namespace SecureOps.Presentation.Api.Options;
/// <summary>
/// Configuration options for exposing the built-in permission management API endpoints.
/// </summary>
public class SecureOpsEndpointsOptions
{
    /// <summary>
    /// Enables or disables endpoints related to adding, removing, and retrieving user-specific permissions.
    /// Default is <c>false</c>.
    /// </summary>
    public bool EnableUserPermissionManagement { get; set; } = false;

    /// <summary>
    /// Enables or disables endpoints for managing global permissions (e.g., registering all available features).
    /// Default is <c>false</c>.
    /// </summary>
    public bool EnableGlobalPermissionManagement { get; set; } = false;

    /// <summary>
    /// Enables or disables the endpoint for listing all registered permissions.
    /// Default is <c>false</c>.
    /// </summary>
    public bool EnableListingAllPermissions { get; set; } = false;

    /// <summary>
    /// The route prefix under which the permission API endpoints will be exposed.
    /// Default is <c>"/api/permissions"</c>.
    /// </summary>
    public string RoutePrefix { get; set; } = "/api/secureOps/permissions";

    /// <summary>
    /// The name of the required claim that a user must have in order to access the permission endpoints.
    /// If this is <c>null</c> or empty, no authentication or authorization is enforced.
    /// If set (e.g., <c>"ManagePermissions"</c>), the request must be authenticated and include this claim.
    /// </summary>
    public string? PermissionClaim => SecureOpsMiddlewareOptions._PermissionClaim;
}


