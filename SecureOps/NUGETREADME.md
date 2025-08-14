# SecureOps

**SecureOps** is a lightweight, extensible, feature-based authorization framework for ASP.NET Core.

SecureOps lets you secure endpoints using named permissions like `"Product.Create"` or `"User.Manage"`, with support for:
- Custom permission stores (InMemory, DB, Redis)
- Caching (InMemory and Redis)
- Configurable user claim resolution (e.g., `sub`, `email`)
- Optional permission management API (Add/Remove/List)
- Minimal API + Attribute Routing
- Zero-dependency core + extensible services

---

## Installation

```bash
dotnet add package SecureOps
```

---

## Getting Started

### 1. Register SecureOps in `Program.cs`

```csharp
builder.Services.AddSecureOps(options =>
{
    options.CacheMode = CacheMode.Memory;
    options.UserIdClaimType = "sub"; // Default is ClaimTypes.Name
    options.AuthenticationOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
});
```

### 2. Use the `[HasPermission]` Attribute

```csharp
[HasPermission("Product.Create")]
[HttpPost("create")]
public IActionResult CreateProduct() => Ok("Permission granted.");
```

---

## Permission Claim API (Optional)

Enable built-in endpoints to manage permissions:

```csharp
app.UseSecureOps(config =>
{
    config.MapPermissionEndpoints(api =>
    {
        api.RoutePrefix = "api/permissions";
        api.PermissionClaim = "ManagePermissions";
        api.EnableUserPermissionManagement = true;
    });
});
```

---

## Redis Support

```csharp
builder.Services.AddSecureOps(options =>
{
    options.CacheMode = CacheMode.Redis;
    options.RedisConfiguration = "localhost:6379";
});
```

> Or register your own `IConnectionMultiplexer` before calling `AddSecureOps`.

---


## Feedback & Contributing
SecureOps is licensed under the MIT License. Bug reports and contributions are welcome at the [GitHub repository](https://github.com/eruzairshafi/SecureOps).