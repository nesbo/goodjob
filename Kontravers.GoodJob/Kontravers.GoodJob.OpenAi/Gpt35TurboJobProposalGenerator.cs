using Azure.AI.OpenAI;
using Kontravers.GoodJob.Domain;
using Kontravers.GoodJob.Domain.Talent;
using Kontravers.GoodJob.Domain.Work;
using Kontravers.GoodJob.Domain.Work.Services;
using Microsoft.Extensions.Logging;

namespace Kontravers.GoodJob.OpenAi;

public class Gpt35TurboJobProposalGenerator : IJobProposalGenerator
{
    private readonly ILogger<Gpt35TurboJobProposalGenerator> _logger;
    private readonly IClock _clock;
    private readonly OpenAiOptions _openAiSettings;

    public Gpt35TurboJobProposalGenerator(ILogger<Gpt35TurboJobProposalGenerator> logger, IClock clock)
    {
        _logger = logger;
        _clock = clock;
        _openAiSettings = new OpenAiOptions
        {
            ApiKey = "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"
        };
    }
    
    public async Task GenerateAsync(Person person, Job job, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Opening OpenAI client");
        var openApiClient = new OpenAIClient(_openAiSettings.ApiKey);
        _logger.LogTrace("Generating job proposal prompt for person [{PersonId}], job title [{JobTitle}]",
            person.Id, job.Title);
        var prompt = GeneratePrompt(person, job);
        _logger.LogInformation("Asking OpenAI to generate job proposal for person [{PersonId}], job title [{JobTitle}]",
            person.Id, job.Title);
        var createdUtc = _clock.UtcNow;
        var response = await openApiClient.GetChatCompletionsAsync(prompt, cancellationToken);
        _logger.LogInformation("Received [{ProposalsCount}] proposals from OpenAI",
            response.Value.Choices.Count);

        foreach (var chatChoice in response.Value.Choices)
        {
            job.AddJobProposal(createdUtc, _clock.UtcNow, chatChoice.Message.Content,
                JobProposalGeneratorType.ChatGpt35Turbo);
        }
    }

    private static ChatCompletionsOptions GeneratePrompt(Person person, Job job)
    {
        return new ChatCompletionsOptions
        {
            DeploymentName = "gpt-3.5-turbo",
            Messages =
            {
                new ChatRequestSystemMessage("You are GoodJob, an upwork proposal writing assistant for freelancers and agencies that takes user inputed profile description and writes a proposal for the job description. You specialize in crafting proposals that are tailored to their unique skills and experiences. Your approach to writing a winning proposal includes: 1. Starting with a quick, straightforward greeting and introduction, followed by a concise restatement of the client’s core need or problem. 2. Including a clear statement that confidently assures the client that their problem can be solved, and indicating readiness to start immediately. 3. Crafting a short, compelling pitch, ideally two to three sentences, that highlights why the freelancer or agency is the perfect fit for the job. 4. Providing a brief yet detailed description of the methods and processes that will be used to approach the project, ensuring excellent service. 5. Attaching relevant documents, files of sample works, or links to a portfolio that demonstrate past projects related to the client’s needs. Write a proposal in the name of the user that follows the previously established rules for writing. Analyse all the information provided to make sure that the proposal is relevant. Make sure that the writing is professional, bullshit-free but in an honest and friendly tone. Avoid: too many words like \"enthusiasm\" and \"thrill\" and \"intricately\" and similar fluff words. Let's keep it professional and straightforward. Persuasion comes from a measured tone, no need to go overboard."),
                new ChatRequestUserMessage("Given I have the following job post title:"),
                new ChatRequestUserMessage(job.Title),
                new ChatRequestUserMessage("And I have the following job post description:"),
                new ChatRequestUserMessage(job.Description),
                new ChatRequestUserMessage("And my name is:"),
                new ChatRequestUserMessage(person.Name),
                // TODO: Include profile text and portfolio items
                new ChatRequestUserMessage("Could you please generate a brief job proposal for me and mark my capabilities to deliver this job expectations?")
            },
            MaxTokens = 1000
        };
    }
}