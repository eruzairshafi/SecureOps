namespace SecureOps.Presentation.Endpoints.Model;
/// <summary>
/// Represents a request for a specific permission within the system.
/// </summary>
/// <remarks>This class is typically used to encapsulate the details of a permission request, such as the name or
/// type of the permission being requested.</remarks>
public class PermissionRequest
{ 
    /// <summary>
    /// Gets or sets the permission associated with the current user or operation.
    /// </summary>
    public string Permission { get; set; } = string.Empty;
}