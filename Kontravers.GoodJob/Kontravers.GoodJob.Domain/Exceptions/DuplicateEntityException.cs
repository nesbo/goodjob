namespace Kontravers.GoodJob.Domain.Exceptions;

public class DuplicateEntityException : Exception
{
    public DuplicateEntityException(string message) : base(message)
    {
        
    }
}