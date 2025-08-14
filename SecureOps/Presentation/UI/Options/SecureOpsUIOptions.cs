using SecureOps.Options;

namespace SecureOps.Presentation.UI.Options;

/// <summary>
/// Represents configuration options for managing and displaying secure operations in a user interface.
/// </summary>
/// <remarks>This class provides options to enable or disable specific permission management features and to
/// configure the claim used for permission validation. These options can be used to customize the behavior of secure
/// operations in the application.</remarks>
public class SecureOpsUIOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether user permission management is enabled.
    /// </summary>
    public bool EnableUserPermissionManagement { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether global permission management is enabled.
    /// </summary>
    public bool EnableGlobalPermissionManagement { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether all permissions can be listed.
    /// </summary>
    public bool EnableListingAllPermissions { get; set; } = false;

    /// <summary>
    /// The route prefix under which the permission API endpoints will be exposed.
    /// Default is <c>"/api/permissions"</c>.
    /// </summary>
    public string RoutePrefix { get; set; } = "/secureOps/permissions";

    /// <summary>
    /// The name of the required claim that a user must have in order to access the permission endpoints.
    /// If this is <c>null</c> or empty, no authentication or authorization is enforced.
    /// If set (e.g., <c>"ManagePermissions"</c>), the request must be authenticated and include this claim.
    /// </summary>
    public string? PermissionClaim => SecureOpsMiddlewareOptions._PermissionClaim;
}
