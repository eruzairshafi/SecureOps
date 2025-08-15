using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SecureOps;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid JWT token.\n\nExample: `Bearer abcde12345...`"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services
    .AddSecureOps(ops =>
    {
        ops.UseMemory(); // (default) Use in-memory storage for permissions and users
        ops.UserIdClaimType = "UserId"; // Set the claim type for user ID
        ops.AuthenticationOptions = new AuthenticationOptions
        {
            DefaultScheme = JwtBearerDefaults.AuthenticationScheme,
        };
    })
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "your-app",
            ValidAudience = "your-app-users",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("a-string-secret-at-least-256-bits-long"))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSecureOps(configure =>
{
    configure.PermissionClaim = null; // Set the claim type for permissions

});
app.UseSecureOpsEndpoints(configure =>
{
    configure.RoutePrefix = "/api/secureOps/permissions"; // Set the route prefix for secure operations endpoints
    configure.EnableUserPermissionManagement = true; // Enable user permission management endpoints
    configure.EnableGlobalPermissionManagement = true; // Enable Global permission management endpoints
    configure.EnableListingAllAvailablePermissions = true; // Enable Listing permission management endpoints
});
app.UseSecureOpsUI(configure =>
{
    configure.RoutePrefix = "/secureOps/permissions"; // Set the route prefix for permission endpoints
    configure.EnableUserPermissionManagement = true; // Enable user permission management endpoints
    configure.EnableListingAllPermissions = true; // Enable Listng permission management endpoints
    configure.EnableGlobalPermissionManagement = true; // Enable Global permission management endpoints
});

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
