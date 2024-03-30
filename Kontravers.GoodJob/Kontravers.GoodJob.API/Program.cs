using Kontravers.GoodJob.API;
using Kontravers.GoodJob.Infra.Shared;
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