using Microsoft.Extensions.DependencyInjection;
using CleanCRUDSolution.Application.Features.Reporting;
using CleanCRUDSolution.Application.Common.Enums;

namespace CleanCRUDSolution.Infrastructure.Reporting
{
    public class PersonReportGeneratorFactory : IPersonReportGeneratorFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public PersonReportGeneratorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IPersonReportGenerator GetGenerator(ReportOptions option)
        {
            return _serviceProvider.GetRequiredKeyedService<IPersonReportGenerator>(option);
        }
    }
}
