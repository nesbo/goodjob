namespace Kontravers.GoodJob.Domain.Messaging.Commands;

public enum JobSourceCommandType : byte
{
    None = 0,
    Upwork,
    Freelancer,
    Guru,
    Indeed
}