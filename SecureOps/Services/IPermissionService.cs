using SecureOps.Authorize.Models;

namespace SecureOps.Services;

/// <summary>
/// Defines methods for managing and verifying user and global permissions.
/// </summary>
/// <remarks>This service provides functionality to check user permissions, assign or remove permissions for
/// specific users,  retrieve permissions for a user or globally, and manage global permissions. It is designed to
/// support asynchronous  operations for scalability in applications requiring permission management.</remarks>
public interface IPermissionService
{
    /// <summary>
    /// Determines whether the specified user has the given permission.
    /// </summary>
    /// <param name="userId">The unique identifier of the user whose permissions are being checked. Cannot be null or empty.</param>
    /// <param name="permission">The name of the permission to check. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the user has the
    /// specified permission; otherwise, <see langword="false"/>.</returns>
    Task<bool> HasPermissionAsync(string userId, string permission);

    /// <summary>
    /// Asynchronously adds a specified permission to the user identified by the given user ID.
    /// </summary>
    /// <remarks>This method does not verify whether the user already has the specified permission.  Ensure
    /// that the user ID and permission are valid before calling this method.</remarks>
    /// <param name="userId">The unique identifier of the user to whom the permission will be added. Cannot be null or empty.</param>
    /// <param name="permission">The name of the permission to add. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddPermissionToUserAsync(string userId, string permission);

    /// <summary>
    /// Removes a specific permission from the specified user asynchronously.
    /// </summary>
    /// <remarks>This method performs the removal operation asynchronously. Ensure that the user and
    /// permission exist before calling this method to avoid exceptions.</remarks>
    /// <param name="userId">The unique identifier of the user from whom the permission will be removed. Cannot be null or empty.</param>
    /// <param name="permission">The name of the permission to remove. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RemovePermissionFromUserAsync(string userId, string permission);

    /// <summary>
    /// Asynchronously retrieves a list of permissions assigned to the specified user.
    /// </summary>
    /// <remarks>This method is typically used to determine the actions or resources a user is authorized to
    /// access. Ensure the <paramref name="userId"/> is valid and corresponds to an existing user in the
    /// system.</remarks>
    /// <param name="userId">The unique identifier of the user whose permissions are being retrieved. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of strings representing the
    /// user's permissions. The list will be empty if the user has no permissions.</returns>
    Task<List<string>> GetUserPermissionsAsync(string userId);

    /// <summary>
    /// Asynchronously retrieves a list of all available permissions.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of strings,  where each
    /// string represents a permission name. If no permissions are available, the list will be empty.</returns>
    Task<List<string>> GetAllPermissionsAsync();


    List<PermissionModel> GetAllAvailablePermissions();

    /// <summary>
    /// Adds a global permission to the system asynchronously.
    /// </summary>
    /// <param name="permission">The name of the permission to add. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddGlobalPermissionAsync(string permission);

    /// <summary>
    /// Removes a global permission from the system.
    /// </summary>
    /// <remarks>This method removes the specified global permission, making it unavailable for use. Ensure
    /// that the permission name provided is valid and exists in the system.</remarks>
    /// <param name="permission">The name of the permission to remove. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RemoveGlobalPermissionAsync(string permission);
}
