using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using RazorLight;
using SecureOps.Presentation.UI.Models;
using SecureOps.Presentation.UI.Options;
using SecureOps.Services;

namespace SecureOps.Presentation.UI;

internal class UIEndpoints
{
    private static readonly RazorLightEngine _engine = new RazorLightEngineBuilder()
     .UseEmbeddedResourcesProject(typeof(UIEndpoints).Assembly, "SecureOps.Presentation.UI.Views")
     .SetOperatingAssembly(typeof(UIEndpoints).Assembly) // 👈 Important
     .UseMemoryCachingProvider()
     .Build();

    private readonly static SecureOpsUIOptions options = new();
    internal static void MapPermissionUIEndpoints(IEndpointRouteBuilder app, Action<SecureOpsUIOptions>? configure = null)
    {
        configure?.Invoke(options);

        var group = app.MapGroup(options.RoutePrefix).WithTags("UIPermissions");

        if (!string.IsNullOrWhiteSpace(options.PermissionClaim))
        {
            group.RequireAuthorization(policy =>
                policy.RequireClaim(options.PermissionClaim));
        }

        if (options.EnableUserPermissionManagement)
        {

            var indexDelegate = async (HttpContext context, IPermissionService service) =>
            {
                var model = new PermissionViewModel
                {
                    Permissions = service.GetAllAvailablePermissions()
                };

                var html = await View("Index",model);
                return Results.Text(html, "text/html");
            };

            group.MapGet("/", indexDelegate);
            group.MapGet("/Index", indexDelegate);
        }

    }

    private static async Task<string> View(string? ViewName = "Index", object? model = null)
    {
        ViewName += ".cshtml";
        return await _engine.CompileRenderAsync(ViewName, model ?? new { });
    }

}