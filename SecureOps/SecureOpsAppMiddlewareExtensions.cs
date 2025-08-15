using Microsoft.AspNetCore.Builder;
using SecureOps.Options;
using SecureOps.Presentation.Api;
using SecureOps.Presentation.Api.Options;
using SecureOps.Presentation.UI;
using SecureOps.Presentation.UI.Options;

namespace SecureOps;

/// <summary>
/// Configures SecureOps endpoints in the web application, enabling features such as user and global  permission
/// management, and listing all permissions.
/// </summary>
/// <remarks>This method registers SecureOps endpoints based on the provided configuration. It applies 
/// authentication and authorization middleware to ensure secure access to the endpoints.   Example usage: <code> var
/// builder = WebApplication.CreateBuilder(args); var app = builder.Build();  app.UseSecureOps(options => {    
/// options.SecureOpsEndpointsOptions.RoutePrefix = "/secure-ops";    
/// options.SecureOpsEndpointsOptions.EnableUserPermissionManagement = true;    
/// options.SecureOpsEndpointsOptions.EnableGlobalPermissionManagement = true; });  app.Run(); </code></remarks>
public static class SecureOpsAppMiddlewareExtensions
{
    /// <summary>
    /// Configures the application to use secure operations middleware, including authentication,  authorization, and
    /// optional permission management endpoints.
    /// </summary>
    /// <remarks>This method sets up authentication and authorization middleware for the application. If 
    /// <paramref name="configure"/> is provided and the <see cref="SecureOpsMiddlewareOptions"/>  include endpoint
    /// options, permission management endpoints will be mapped to the application.  Use this method to enable secure
    /// operations in your application, including features such as  user and global permission management, and listing
    /// all permissions. Ensure that the application  has appropriate authentication and authorization mechanisms
    /// configured.</remarks>
    /// <param name="app">The <see cref="WebApplication"/> instance to configure.</param>
    /// <param name="configure">An optional delegate to configure <see cref="SecureOpsMiddlewareOptions"/>. If provided,  this delegate allows
    /// customization of secure operations settings, such as endpoint routing  and permission management options.</param>
    /// <returns>The configured <see cref="WebApplication"/> instance.</returns>
    public static WebApplication UseSecureOps(
        this WebApplication app,
        Action<SecureOpsMiddlewareOptions>? configure = null)
    {

        var ops = new SecureOpsMiddlewareOptions();
        configure?.Invoke(ops);

        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }

    /// <summary>
    /// Configures the application to use secure operational endpoints with optional customization.
    /// </summary>
    /// <remarks>This method sets up secure operational endpoints for the application.  Use the <paramref
    /// name="configure"/> parameter to customize the behavior of these endpoints.</remarks>
    /// <param name="app">The <see cref="WebApplication"/> instance to configure.</param>
    /// <param name="configure">An optional delegate to configure the <see cref="SecureOpsEndpointsOptions"/> for the secure endpoints. If not
    /// provided, default options will be used.</param>
    /// <returns>The <see cref="WebApplication"/> instance, allowing for further configuration.</returns>
    public static WebApplication UseSecureOpsEndpoints(
        this WebApplication app,
        Action<SecureOpsEndpointsOptions>? configure = null)
    {

        Endpoints.MapPermissionEndpoints(app, configure);

        return app;
    }


    public static WebApplication UseSecureOpsUI(
        this WebApplication app,
        Action<SecureOpsUIOptions>? configure = null)
    {
        UIEndpoints.MapPermissionUIEndpoints(app, configure);

        return app;
    }
}
