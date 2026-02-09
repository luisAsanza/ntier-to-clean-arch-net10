namespace CleanCRUDSolution.Infrastructure.FileReader
{
    public interface IXlsxSafetyValidator
    {
        Task ValidateXlsxContainerAsync(Stream stream, CancellationToken ct = default);
    }
}
