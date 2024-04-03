using IdentityModel;
using Kontravers.GoodJob.API;
using Kontravers.GoodJob.Infra.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddControllers();

services.AddEndpointsApiExplorer();
services.AddGoodJobServices().AddBrighterRegistrations();
services.AddCors(options =>
{
    options.AddPolicy("AllowAllCorsPolicy", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
services.AddAuthorizationBuilder()
    .AddPolicy(AuthConstants.PersonTalentAuthorisation, policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", AuthConstants.PersonTalentScope);
    })
    .AddPolicy(AuthConstants.PersonWorkAuthorisation, policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", AuthConstants.PersonWorkScope);
    });

var configuration = builder.Configuration;
var authenticationAuthority = configuration["Auth:Authority"];
if (authenticationAuthority is null)
{
    throw new InvalidOperationException("Authority is not configured.");
}

services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.ForwardChallenge = "oidc";
        options.Events.OnRedirectToAccessDenied = ctx =>
        {
            ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        };
    })
    .AddOpenIdConnect("oidc", options =>
    {
        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.Authority = authenticationAuthority;
        options.ClientId = "goodjob-api";
        options.ClientSecret = "goodjob-api-secret";
        options.ResponseType = OpenIdConnectResponseType.Code;
        options.ClaimActions.MapAll();
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.TokenValidationParameters.NameClaimType = JwtClaimTypes.Name;
        options.TokenValidationParameters.RoleClaimType = JwtClaimTypes.Role;
        
        options.RequireHttpsMetadata = false;
        
        options.Events.OnRedirectToIdentityProvider = ctx =>
        {
            if (ctx.Request.Path == "/account/signin" || ctx.Request.Path == "/account/signout")
                return Task.CompletedTask;

            ctx.HandleResponse();
            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
    });

// services.AddAuthentication("Bearer")
//     .AddJwtBearer("Bearer", options =>
//     {
//         options.Authority = authenticationAuthority;
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateAudience = false
//         };
//         options.RequireHttpsMetadata = false;
//     });
var swaggerConfig = new SwaggerConfig(configuration);
services.AddSingleton(swaggerConfig);
services.AddSwaggerGen(swaggerConfig.ConfigureSwaggerGen);

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(swaggerConfig.ConfigureUI);
}

app.AddGoodJobMinimalApis();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers()
    .RequireAuthorization(options => options.RequireAuthenticatedUser());

await app.RunAsync();

public class SwaggerConfig
{
    private readonly string _authUrl;
    private readonly string _apiScope;
    private readonly string _tokenUrl;
    private readonly string _clientId;

    public SwaggerConfig(IConfiguration config)
    {
        var auth = config["Auth:Authority"];
        _authUrl = $"{auth}/connect/authorize";
        _tokenUrl = $"{auth}/connect/token";
        _clientId = "goodjob-api";
        _apiScope = "openid profile person-talent person-work";
    }

    public void ConfigureSwaggerGen(SwaggerGenOptions options)
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "api",
            Version = "v1"
        });

        // Add OAuth support.
        // Remember to add "https://(host)/swagger/oauth2-redirect.html" to the list
        // of Redirect URIs in the Azure app registration (Authentication).
        options.AddSecurityDefinition("oauth2", CreateSecurityScheme());
        options.AddSecurityRequirement(CreateSecurityRequirement());
    }

    public void ConfigureUI(SwaggerUIOptions options)
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "api v1");
        options.OAuthClientId(_clientId);
        options.OAuthUsePkce();
        options.OAuthScopeSeparator(" ");
    }

    private OpenApiSecurityScheme CreateSecurityScheme()
    {
        return new()
        {
            Description = "OAuth 2.0",
            Name = "oauth2",
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new System.Uri(_authUrl),
                    TokenUrl = new System.Uri(_tokenUrl),
                    Scopes = new Dictionary<string, string>
                    {
                        {_apiScope, "Read"}
                    }
                }
            }
        };
    }

    private OpenApiSecurityRequirement CreateSecurityRequirement()
    {
        return new()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "oauth2"
                    }
                },
                new[] {_apiScope}
            }
        };
    }

}