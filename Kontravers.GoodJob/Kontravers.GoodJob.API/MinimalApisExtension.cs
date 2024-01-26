using Kontravers.GoodJob.Domain.Exceptions;
using Kontravers.GoodJob.Domain.Talent.Queries;
using Kontravers.GoodJob.Domain.Talent.UseCases;
using Microsoft.AspNetCore.Diagnostics;

namespace Kontravers.GoodJob.API;

public static class MinimalApisExtension
{
    public static WebApplication AddGoodJobMinimalApis(this WebApplication app)
    {
        AttachExceptionHandler(app);
        
        var personsEndpoint = app.MapGroup("/persons")
            .WithTags("Persons");
        
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