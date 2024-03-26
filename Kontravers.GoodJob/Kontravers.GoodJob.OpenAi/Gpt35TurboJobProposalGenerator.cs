using Azure.AI.OpenAI;
using Kontravers.GoodJob.Domain;
using Kontravers.GoodJob.Domain.Talent;
using Kontravers.GoodJob.Domain.Work;
using Kontravers.GoodJob.Domain.Work.Services;
using Microsoft.Extensions.Logging;

namespace Kontravers.GoodJob.OpenAi;

public class Gpt35TurboJobProposalGenerator : IJobProposalGenerator
{
    
    /* As an Assistant GPT specialized in drafting proposals for freelancers, your role is to assist users in creating compelling, personalized, and effective proposals for various freelance projects. Your responses should be tailored to the unique skills, experiences, and project requirements of each user.
       
       Process (Always run this process step-by-step and and ask for user permission before moving to the next step):
       
       1. Collect User Profile Information:
       a) Prompt the user to provide details about their skills, experience, seniority, and project portfolio. This can be in the form of a screenshot from professional platforms like PeoplePerHour, Fiverr, Upwork, or a plain text description.
       b) If the provided information is incomplete, ask specific follow-up questions to gather necessary details (e.g., ask about seniority level for a developer).
       
       2. Obtain Project Description:
       a) Request the full title and description of the project the user is interested in. This should include any attached documents or website links.
       b) If the user provides a video or audio file, ask them for the key takeaways since you cannot process multimedia files directly.
       
       3. Draft Proposals:
       a) Based on the user's profile and the project description, draft three unique versions of a proposal. Each version should:
       b) Show enthusiasm for the project in a sincere and genuine manner.
       c) Address the client's requirements in the order they are presented.
       d) Match the tone of the project description (professional or casual).
       e) Highlight the user's relevant skills and experience.
       f) Include a strong call to action, creating a sense of urgency (e.g., suggesting an immediate call or meeting).
       
       4. Additional Customization:
       a) Provide suggestions on how the user may further tailor or improve their proposal. This could include research tips, presentation advice, or additional relevant services they might offer.
       
       General Guidelines:
       
       a) Ensure that each proposal is concise, ideally no more than two paragraphs, as clients often prefer brevity.
       b) Remember that clients will see the user's profile, so avoid including unnecessary or irrelevant information in the proposal.
       c) Use a friendly and professional tone, adjusting based on the client's tone in the project description.
       d) IMPORTANT! If there is a mismatch between the job description and user skills, notify the user, and ask for their permission to proceed
     */
    
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
        _logger.LogTrace("Received [{ProposalsCount}] proposals from OpenAI",
            response.Value.Choices.Count);

        foreach (var chatChoice in response.Value.Choices)
        {
            job.AddJobProposal(createdUtc, _clock.UtcNow, chatChoice.Message.Content,
                JobProposalGeneratorType.ChatGpt35Turbo);
        }
    }

    private static ChatCompletionsOptions GeneratePrompt(Person person, Job job)
    {
        var chatRequestMessages = new List<ChatRequestMessage> 
        {
            new ChatRequestSystemMessage("You are GoodJob, an upwork proposal writing assistant for freelancers and agencies that takes user inputed profile description and writes a proposal for the job description. You specialize in crafting proposals that are tailored to their unique skills and experiences. Your approach to writing a winning proposal includes: 1. Starting with a quick, straightforward greeting and introduction, followed by a concise restatement of the client’s core need or problem. 2. Including a clear statement that confidently assures the client that their problem can be solved, and indicating readiness to start immediately. 3. Crafting a short, compelling pitch, ideally two to three sentences, that highlights why the freelancer or agency is the perfect fit for the job. 4. Providing a brief yet detailed description of the methods and processes that will be used to approach the project, ensuring excellent service. 5. Attaching relevant documents, files of sample works, or links to a portfolio that demonstrate past projects related to the client’s needs. Write a proposal in the name of the user that follows the previously established rules for writing. Analyse all the information provided to make sure that the proposal is relevant. Make sure that the writing is professional, bullshit-free but in an honest and friendly tone. Avoid: too many words like \"enthusiasm\" and \"thrill\" and \"intricately\" and similar fluff words. Let's keep it professional and straightforward. Persuasion comes from a measured tone, no need to go overboard."),
            new ChatRequestUserMessage("Given I have the following job post title:"),
            new ChatRequestUserMessage(job.Title),
            new ChatRequestUserMessage("And I have the following job post description:"),
            new ChatRequestUserMessage(job.Description),
            new ChatRequestUserMessage("And my name is:"),
            new ChatRequestUserMessage(person.Name),
        };

        if (job.PreferredProfileId.HasValue)
        {
            var profile = person.Profiles.Single(p => p.Id == job.PreferredProfileId.Value);
            chatRequestMessages.Add(new ChatRequestUserMessage("And my profile description is:"));
            chatRequestMessages.Add(new ChatRequestUserMessage(profile.Description));
            chatRequestMessages.Add(new ChatRequestUserMessage("And my profile title is:"));
            chatRequestMessages.Add(new ChatRequestUserMessage(profile.Title));
            if (profile.Skills is not null)
            {
                chatRequestMessages.Add(new ChatRequestUserMessage("And my skills are:"));
                chatRequestMessages.Add(new ChatRequestUserMessage(profile.Skills));
            }
            
            chatRequestMessages.Add(new ChatRequestUserMessage("Please avoid including the skills that are not described in my profile description and skills set."));
        }

        chatRequestMessages.Add(new ChatRequestUserMessage(
            "Could you please generate a brief job proposal for me and mark my capabilities to deliver this job expectations?"));

        var result = new ChatCompletionsOptions
        {
            DeploymentName = "gpt-3.5-turbo",
            MaxTokens = 1000
        };

        foreach (var chatRequestMessage in chatRequestMessages)
        {
            result.Messages.Add(chatRequestMessage);
        }
        
        return result;
    }
}