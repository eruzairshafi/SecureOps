using SecureOps.Authorize;
using SecureOps.Authorize.Models;
using System.Reflection;
using System.Security;

namespace SecureOps.Services;

internal abstract class BasePermissionService : IPermissionService
{
    private List<PermissionModel>? _availablePermissions = null;
    public abstract Task AddGlobalPermissionAsync(string permission);
    public abstract Task AddPermissionToUserAsync(string userId, string permission);
    public List<PermissionModel> GetAllAvailablePermissions()
    {
        if (_availablePermissions != null)
        {
            return _availablePermissions;
        }   

        _availablePermissions = [.. AppDomain.CurrentDomain
                                        .GetAssemblies()
                                        .SelectMany(a => a.GetTypes())
                                        .SelectMany(t => t.GetCustomAttributes<HasPermissionAttribute>(true)
                                                            .Concat(t.GetMethods()
                                                                    .SelectMany(m => m.GetCustomAttributes<HasPermissionAttribute>(true))))
                                        .Select(attr => attr.Permission)
                                        .Distinct()];

        return _availablePermissions;    
    }
    public abstract Task<List<string>> GetAllPermissionsAsync();
    public abstract Task<List<string>> GetUserPermissionsAsync(string userId);
    public abstract Task<bool> HasPermissionAsync(string userId, string permission);
    public abstract Task RemoveGlobalPermissionAsync(string permission);
    public abstract Task RemovePermissionFromUserAsync(string userId, string permission);
}
