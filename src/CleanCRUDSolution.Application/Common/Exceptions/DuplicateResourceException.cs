namespace CleanCRUDSolution.Application.Common.Exceptions
{
    public class DuplicateResourceException : Exception
    {
        public DuplicateResourceException(string message) : base(message)
        {
        }
    }
}
