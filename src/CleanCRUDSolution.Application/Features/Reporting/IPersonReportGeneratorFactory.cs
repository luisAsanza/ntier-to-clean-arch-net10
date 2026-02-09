using CleanCRUDSolution.Application.Common.Enums;

namespace CleanCRUDSolution.Application.Features.Reporting
{
    /// <summary>
    /// Defines a contract for generating strategy-specific report generators based on provided report options.
    /// </summary>
    public interface IPersonReportGeneratorFactory
    {
        IPersonReportGenerator GetGenerator(ReportOptions option);
    }
}
