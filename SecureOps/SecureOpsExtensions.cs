using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SecureOps.Services;
using SecureOps.Services.Cache;
using SecureOps.Services.Cache.Enums;
using SecureOps.Services.Cache.Options;
using StackExchange.Redis;

namespace SecureOps;

/// <summary>
/// Provides extension methods for configuring secure operations in an application, including authentication, 
/// authorization, and permission management.
/// </summary>
/// <remarks>The <see cref="SecureOpsExtensions"/> class contains methods to integrate secure operations into an
/// application's  service collection. These methods allow for flexible configuration of authentication schemes,
/// authorization policies,  and permission management using either in-memory or Redis-based caching. Additionally,
/// custom implementations of  <see cref="IPermissionStore"/> can be registered to tailor permission management to
/// specific requirements.</remarks>
public static class SecureOpsExtensions
{
    /// <summary>
    /// Configures secure operations for the application by setting up authentication, authorization,  and permission
    /// management services.
    /// </summary>
    /// <remarks>This method sets up authentication and authorization services, as well as permission
    /// management  using either in-memory or Redis-based caching, depending on the configuration provided in  <paramref
    /// name="configure"/>.   If Redis caching is selected, the <see cref="SecureOpsOptions.RedisConfiguration"/> must
    /// be set,  or an <see cref="InvalidOperationException"/> will be thrown. In-memory caching is used by default  if
    /// Redis is not configured.  The method also registers default authentication schemes if specified in  <see
    /// cref="SecureOpsOptions.AuthenticationOptions"/>.</remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the secure operations services will be added.</param>
    /// <param name="configure">An optional delegate to configure <see cref="SecureOpsOptions"/>. If not provided, default options will be used.</param>
    /// <returns>An <see cref="AuthenticationBuilder"/> that can be used to further configure authentication schemes.</returns>
    /// <exception cref="InvalidOperationException">Thrown if Redis caching is selected and <see cref="SecureOpsOptions.RedisConfiguration"/> is not set.</exception>
    public static AuthenticationBuilder AddSecureOps(this IServiceCollection services, Action<SecureOpsOptions>? configure = null)
    {
        var options = new SecureOpsOptions();
        configure?.Invoke(options);
        
        services.TryAddSingleton<IPermissionStore, InMemoryPermissionStore>();

        if (options.CacheMode == CacheMode.Redis)
        {
            services.TryAddSingleton<IConnectionMultiplexer>(sp =>
            {
                // Use existing if already added, else create using configuration string
                if (!string.IsNullOrEmpty(options.RedisConfiguration))
                {
                    return ConnectionMultiplexer.Connect(options.RedisConfiguration);
                }

                throw new InvalidOperationException("RedisConfiguration must be set if IConnectionMultiplexer is not already registered.");
            });

            services.AddScoped<IPermissionService, RedisCachedPermissionService>();
        }

        else
        {
            services.AddMemoryCache();
            services.AddScoped<IPermissionService, CachedPermissionService>();
        }
        // Register Authentication
        var builder = services.AddAuthentication(auth =>
        {
            if (options.AuthenticationOptions != null)
            {
                auth.DefaultScheme = options.AuthenticationOptions.DefaultScheme;
                auth.DefaultChallengeScheme = options.AuthenticationOptions.DefaultChallengeScheme;
                auth.DefaultAuthenticateScheme = options.AuthenticationOptions.DefaultAuthenticateScheme;
                auth.DefaultForbidScheme = options.AuthenticationOptions.DefaultForbidScheme;
                auth.DefaultSignInScheme = options.AuthenticationOptions.DefaultSignInScheme;
                auth.DefaultSignOutScheme = options.AuthenticationOptions.DefaultSignOutScheme;
            }
        });

        services.AddAuthorization();
        services.AddSingleton(options);
        return builder;
    }
    
    /// <summary>
    /// Adds SecureOps authentication to the specified service collection.
    /// </summary>
    /// <remarks>This method configures SecureOps authentication with the specified default scheme. Use this
    /// method to integrate SecureOps authentication into your application's service collection.</remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the SecureOps authentication is added.</param>
    /// <param name="DefaultScheme">The default authentication scheme to be used. This value cannot be null or empty.</param>
    /// <returns>An <see cref="AuthenticationBuilder"/> that can be used to further configure authentication.</returns>
    public static AuthenticationBuilder AddSecureOps(this IServiceCollection services, string DefaultScheme)
    {
        return services.AddSecureOps(configure =>
        {
            configure.AuthenticationOptions.DefaultScheme = DefaultScheme;
        });
    }
    
    /// <summary>
    /// Adds SecureOps authentication and authorization services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <remarks>This method registers the specified <typeparamref name="TStore"/> as the implementation of
    /// <see cref="IPermissionStore"/> in the service collection. It also sets up SecureOps authentication and
    /// authorization using the provided configuration.</remarks>
    /// <typeparam name="TStore">The type of the permission store to be used for managing permissions. Must implement <see
    /// cref="IPermissionStore"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the SecureOps services will be added.</param>
    /// <param name="configure">A delegate to configure the <see cref="SecureOpsOptions"/> for SecureOps.</param>
    /// <returns>An <see cref="AuthenticationBuilder"/> that can be used to further configure authentication.</returns>
    public static AuthenticationBuilder AddSecureOps<TStore>(this IServiceCollection services,Action<SecureOpsOptions> configure)
        where TStore : class, IPermissionStore
    {
        services.AddScoped<IPermissionStore, TStore>();

        return services.AddSecureOps(configure);
    }
    
    /// <summary>
    /// Adds secure operations authentication and authorization services to the specified service collection.
    /// </summary>
    /// <remarks>This method registers the specified <typeparamref name="TStore"/> as the implementation of 
    /// <see cref="IPermissionStore"/> in the service collection. It also configures secure operations  authentication
    /// using the provided default scheme.</remarks>
    /// <typeparam name="TStore">The type of the permission store to be used for managing permissions.  Must implement <see
    /// cref="IPermissionStore"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the secure operations services will be added.</param>
    /// <param name="DefaultScheme">The default authentication scheme to be used for secure operations.</param>
    /// <returns>An <see cref="AuthenticationBuilder"/> that can be used to further configure authentication.</returns>
    public static AuthenticationBuilder AddSecureOps<TStore>(this IServiceCollection services,string DefaultScheme)
        where TStore : class, IPermissionStore
    {
        services.AddScoped<IPermissionStore, TStore>();

        return services.AddSecureOps(DefaultScheme);
    }
}
