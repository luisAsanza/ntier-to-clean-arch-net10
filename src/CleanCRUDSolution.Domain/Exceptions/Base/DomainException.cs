namespace CleanCRUDSolution.Domain.Exceptions.Base
{
    public class DomainException : Exception
    {
        public DomainException(string message)
            : base(message)
        {
            
        }
    }
}
