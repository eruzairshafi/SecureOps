namespace SecureOps.Authorize.Models;

public class PermissionModel(string Permission, string Description, string Category)
{
    public string Name { get; } = Permission ?? throw new ArgumentNullException(nameof(Permission), "Permission cannot be null or empty.");
    public string Description { get; } = Description ?? throw new ArgumentNullException(nameof(Description), "Description cannot be null or empty.");
    public string Category { get; } = Category ?? throw new ArgumentNullException(nameof(Category), "Category cannot be null or empty.");
}
