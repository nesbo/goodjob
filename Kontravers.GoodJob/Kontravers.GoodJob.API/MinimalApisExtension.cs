using Kontravers.GoodJob.API.Requests.Talent;
using Kontravers.GoodJob.Domain;
using Kontravers.GoodJob.Domain.Exceptions;
using Kontravers.GoodJob.Domain.Talent.Queries;
using Kontravers.GoodJob.Domain.Talent.UseCases;
using Microsoft.AspNetCore.Diagnostics;
using Paramore.Brighter;

namespace Kontravers.GoodJob.API;

public static class MinimalApisExtension
{
    public static WebApplication AddGoodJobMinimalApis(this WebApplication app)
    {
        AttachExceptionHandler(app);

        app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        
        var personsEndpoint = app.MapGroup("/persons")
            .WithTags("Talent");
        
        personsEndpoint
            .MapGet("/", (ListPersonsQueryHandler handler, CancellationToken cancellationToken) =>
                handler.ListAsync(new ListPersonsQuery("1", 1, 10), cancellationToken))
            .Produces<PersonListViewModel>();

        personsEndpoint
            .MapGet("{personId}",
                (GetPersonQueryHandler handler, string personId, CancellationToken cancellationToken) =>
                    handler.GetAsync(new GetPersonQuery(personId), cancellationToken))
            .Produces<PersonViewModel>();

        personsEndpoint
            .MapGet("{personId}/upwork-rss-feeds/{upworkRssFeedId}",
                (GetPersonUpworkRssFeedQueryHandler handler, string personId, string upworkRssFeedId,
                        CancellationToken cancellationToken) =>
                    handler.GetAsync(new GetPersonUpworkRssFeedQuery(personId, upworkRssFeedId), cancellationToken))
            .Produces<PersonUpworkRssFeedViewModel>();

        personsEndpoint.MapPut("{personId}/upwork-rss-feeds/{upworkRssFeedId}",
            (IAmACommandProcessor commandProcessor, string personId, string upworkRssFeedId,
                    UpdatePersonUpworkRssFeedRequest request, IClock clock, CancellationToken cancellationToken) =>
                commandProcessor.SendAsync(request.ToCommand(clock, personId, upworkRssFeedId),
                    cancellationToken: cancellationToken));
        
        personsEndpoint.MapPost("{personId}/profiles",
            (IAmACommandProcessor commandProcessor, string personId,
                    CreatePersonProfileRequest request, IClock clock, CancellationToken cancellationToken) =>
                commandProcessor.SendAsync(request.ToCommand(clock, personId), cancellationToken: cancellationToken));
        
        personsEndpoint.MapGet("{personId}/profiles/{profileId}",
            (GetPersonProfileQueryHandler handler, string personId, string profileId,
                    CancellationToken cancellationToken) =>
                handler.GetAsync(new GetPersonProfileQuery(personId, profileId), cancellationToken))
            .Produces<PersonProfileViewModel>();
        
        personsEndpoint.MapPut("{personId}/profiles/{profileId}",
            (IAmACommandProcessor commandProcessor, string personId, string profileId,
                    UpdatePersonProfileRequest request, IClock clock, CancellationToken cancellationToken) =>
                commandProcessor.SendAsync(request.ToCommand(clock, personId, profileId),
                    cancellationToken: cancellationToken));
        
        

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
                if (exception is NotFoundException)
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        error = exception.Message
                    });
                }
            });
        });
    }
}