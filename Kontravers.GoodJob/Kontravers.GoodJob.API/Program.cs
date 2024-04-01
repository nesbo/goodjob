using IdentityModel;
using Kontravers.GoodJob.API;
using Kontravers.GoodJob.Infra.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddControllers();

services.AddEndpointsApiExplorer();
services.AddGoodJobServices().AddBrighterRegistrations();
services.AddCors();
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
        options.Authority = authenticationAuthority;
        options.ResponseType = OpenIdConnectResponseType.Code;
        options.ClaimActions.MapAll();
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.TokenValidationParameters.NameClaimType = JwtClaimTypes.Name;
        options.TokenValidationParameters.RoleClaimType = JwtClaimTypes.Role;

        options.Events.OnRedirectToIdentityProvider = ctx =>
        {
            if (ctx.Request.Path == "/account/signin" || ctx.Request.Path == "/account/signout")
                return Task.CompletedTask;

            ctx.HandleResponse();
            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
    });

services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = authenticationAuthority;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
        options.RequireHttpsMetadata = false;
    });
services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddGoodJobMinimalApis();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers()
    .RequireAuthorization(options => options.RequireAuthenticatedUser());

await app.RunAsync();