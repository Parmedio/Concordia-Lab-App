using BusinessLogic.DTOs.ReportDto;

using ReportSender.ReportDto;

namespace ReportSender
{
    public interface IConcordiaReportRunner
    {
        Task<string> Run(IEnumerable<ExperimentForReportDto> experiments, IEnumerable<ScientistForReportDto> scientists, bool sendMail);
    }
}