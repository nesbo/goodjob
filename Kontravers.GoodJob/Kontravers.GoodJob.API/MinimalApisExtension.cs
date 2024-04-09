using System.Security.Claims;
using Kontravers.GoodJob.API.Requests.Talent;
using Kontravers.GoodJob.Domain;
using Kontravers.GoodJob.Domain.Exceptions;
using Kontravers.GoodJob.Domain.Talent.Queries;
using Kontravers.GoodJob.Domain.Talent.UseCases;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Options;
using Paramore.Brighter;

namespace Kontravers.GoodJob.API;

public static class MinimalApisExtension
{
    public static WebApplication AddGoodJobMinimalApis(this WebApplication app)
    {
        AttachExceptionHandler(app);

        app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

        var personEndpoint = app
            .MapGroup("/person")
            .WithTags("Talent")
            .RequireCors("WebApiCorsPolicy")
            .RequireAuthorization(builder => builder
                .RequireAuthenticatedUser());

        personEndpoint
            .MapGet("/",
                (GetPerson handler, CancellationToken cancellationToken, HttpContext context) =>
                {
                    var userId = GetUserId(context);
                    return handler.GetAsync(new GetPersonByUserIdQuery(userId), cancellationToken);
                })
            .Produces<PersonViewModel>();

        personEndpoint
            .MapGet("/upwork-rss-feeds/{upworkRssFeedId}",
                (GetPersonUpworkRssFeed handler, string upworkRssFeedId,
                    CancellationToken cancellationToken, HttpContext context) =>
                {
                    var userId = GetUserId(context);
                    return handler.GetAsync(new GetPersonUpworkRssFeedByUserIdQuery(userId, upworkRssFeedId),
                        cancellationToken);
                })
            .Produces<PersonUpworkRssFeedViewModel>();

        personEndpoint.MapPut("/upwork-rss-feeds/{upworkRssFeedId}",
            (IAmACommandProcessor commandProcessor, string upworkRssFeedId,
                UpdatePersonUpworkRssFeedRequest request, IClock clock, CancellationToken cancellationToken,
                HttpContext context) =>
            {
                var userId = GetUserId(context);
                return commandProcessor.SendAsync(request.ToCommand(clock, userId, upworkRssFeedId),
                    cancellationToken: cancellationToken);
            });
        
        personEndpoint.MapPost("/profiles",
            (IAmACommandProcessor commandProcessor, CreatePersonProfileRequest request,
                IClock clock, CancellationToken cancellationToken, HttpContext context) =>
            {
                var userId = GetUserId(context);
                return commandProcessor.SendAsync(request.ToCommand(clock, userId),
                    cancellationToken: cancellationToken);
            });

        personEndpoint.MapGet("/profiles/{profileId}",
                (GetPersonProfile handler, string profileId, CancellationToken cancellationToken,
                    HttpContext context) =>
                {
                    var userId = GetUserId(context);
                    return handler.GetAsync(new GetPersonProfileQuery(userId, profileId), cancellationToken);
                })
            .Produces<PersonProfileViewModel>();

        personEndpoint.MapPut("/profiles/{profileId}",
            (IAmACommandProcessor commandProcessor, string personId,
                UpdatePersonProfileRequest request, IClock clock, CancellationToken cancellationToken,
                HttpContext context) =>
            {
                var userId = GetUserId(context);
                return commandProcessor.SendAsync(request.ToCommand(clock, personId, userId),
                    cancellationToken: cancellationToken);
            });

        personEndpoint.MapPost("/upwork-rss-feeds",
            (IAmACommandProcessor commandProcessor, CreatePersonUpworkRssFeedRequest request,
                IClock clock, CancellationToken cancellationToken, HttpContext context) =>
            {
                var userId = GetUserId(context);
                return commandProcessor.SendAsync(request.ToCommand(clock, userId),
                    cancellationToken: cancellationToken);
            });

        return app;
    }
    
    private static void AttachExceptionHandler(WebApplication app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var exceptionHandlerPathFeature =
                    context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature?.Error;
                switch (exception)
                {
                    case NotFoundException:
                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        await context.Response.WriteAsJsonAsync(new
                        {
                            error = exception.Message
                        });
                        break;
                    case UnauthorizedAccessException:
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        break;
                    default:
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        await context.Response.WriteAsJsonAsync(new
                        {
                            error = $"Internal server error - {exception?.Message}"
                        });
                        break;
                }
            });
        });
    }
    
    private static string GetUserId(HttpContext context)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new UnauthorizedAccessException("User not authenticated");
        }

        return userId;
    }
}

public class CorsOptionsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ICorsService _corsService;
    private readonly ICorsPolicyProvider _corsPolicyProvider;
    private readonly CorsOptionsMiddlewareOptions _options;

    public CorsOptionsMiddleware(RequestDelegate next, ICorsService corsService,
        ICorsPolicyProvider corsPolicyProvider,
        IOptions<CorsOptionsMiddlewareOptions> options)
    {
        _next = next;
        _corsService = corsService;
        _corsPolicyProvider = corsPolicyProvider;
        _options = options.Value;
    }

    public Task Invoke(HttpContext context)
    {
        return BeginInvoke(context);
    }

    private async Task BeginInvoke(HttpContext context)
    {
        if (HttpMethods.IsOptions(context.Request.Method))
        {
            var policy = await _corsPolicyProvider.GetPolicyAsync(context, _options.CorsPolicyName);
            if (policy == null)
                throw new Exception($"Could not find CORS policy with name '{_options.CorsPolicyName}'");

            var result = _corsService.EvaluatePolicy(context, policy);
            _corsService.ApplyResult(result, context.Response);
            return;
        }

        await _next(context);
    }
}

public class CorsOptionsMiddlewareOptions
{
    public string CorsPolicyName { get; set; }
}

public static class CorsOptionsMiddlewareExtensions
{
    public static IServiceCollection AddCorsOptions(this IServiceCollection services, string corsPolicyName)
    {
        services.Configure<CorsOptionsMiddlewareOptions>(options =>
        {
            options.CorsPolicyName = corsPolicyName;
        });

        return services;
    }

    public static IApplicationBuilder UseCorsOptions(this IApplicationBuilder app)
    {
        return app.UseMiddleware<CorsOptionsMiddleware>();
    }
}