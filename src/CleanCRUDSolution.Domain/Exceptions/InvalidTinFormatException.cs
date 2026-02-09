using CleanCRUDSolution.Domain.Exceptions.Base;

namespace CleanCRUDSolution.Domain.Exceptions
{
    public sealed class InvalidTinFormatException(string tin) : DomainException($"The provided Tax Identification Number (TIN) format is invalid. Tin: {tin}")
    {
        public string Tin { get; set; } = tin;
    }
}
