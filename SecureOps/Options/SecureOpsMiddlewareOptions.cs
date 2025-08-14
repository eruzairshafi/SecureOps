namespace SecureOps.Options;

/// <summary>
/// 
/// </summary>
public class SecureOpsMiddlewareOptions
{
    /// <summary>
    /// The name of the required claim that a user must have in order to access the permission endpoints.
    /// If this is <c>null</c> or empty, no authentication or authorization is enforced.
    /// If set (e.g., <c>"ManagePermissions"</c>), the request must be authenticated and include this claim.
    /// </summary>

    public string? PermissionClaim { set { SecureOpsMiddlewareOptions._PermissionClaim = value; } }

    /// <summary>
    /// The name of the required claim that a user must have in order to access the permission endpoints.
    /// If this is <c>null</c> or empty, no authentication or authorization is enforced.
    /// If set (e.g., <c>"ManagePermissions"</c>), the request must be authenticated and include this claim.
    /// </summary>
    public static string? _PermissionClaim { get; set; } = "ManagePermissions";
}

