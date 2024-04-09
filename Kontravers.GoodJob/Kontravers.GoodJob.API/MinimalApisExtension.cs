using System.Security.Claims;
using Kontravers.GoodJob.API.Requests.Talent;
using Kontravers.GoodJob.Domain;
using Kontravers.GoodJob.Domain.Exceptions;
using Kontravers.GoodJob.Domain.Talent.Queries;
using Kontravers.GoodJob.Domain.Talent.UseCases;
using Kontravers.GoodJob.Infra.Shared;
using Microsoft.AspNetCore.Diagnostics;
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
            .RequireAuthorization(builder => builder
                .RequireAuthenticatedUser());
                //.RequireClaim("scope", AuthConstants.PersonTalentScope));

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