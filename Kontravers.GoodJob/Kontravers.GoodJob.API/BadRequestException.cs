namespace Kontravers.GoodJob.API;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message) { }
}