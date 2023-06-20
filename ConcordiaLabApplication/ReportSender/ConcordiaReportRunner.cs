using BusinessLogic.DTOs.ReportDto;

using Microsoft.Extensions.Logging;

using ReportSender.FileSystemManager.Abstract;
using ReportSender.ReportDto;

namespace ReportSender;

public class ConcordiaReportRunner : IConcordiaReportRunner
{
    private static int reportNumber = 0;
    private readonly IFileSystemDocumentManager _systemDocumentManager;
    private readonly ILogger<ConcordiaReportRunner> _logger;

    public ConcordiaReportRunner(IFileSystemDocumentManager systemDocumentManager, ILogger<ConcordiaReportRunner> logger)
    {
        _systemDocumentManager = systemDocumentManager;
        _logger = logger;
    }

    public async Task Run(IEnumerable<ExperimentForReportDto> experiments, IEnumerable<ScientistForReportDto> scientists)
    {
        _logger.LogInformation("Initiating Report Generation...");
        await Task.Run(() =>
        {
            var path = _systemDocumentManager.CheckAndGenerateFileStructure();
            ConcordiaReportBuilder builder = new ConcordiaReportBuilder(experiments, scientists, reportNumber);
            reportNumber++;
            builder.Build().Build(path);
        });
        _logger.LogInformation("Report generated.");
    }
}
