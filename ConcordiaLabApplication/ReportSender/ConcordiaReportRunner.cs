using BusinessLogic.DTOs.ReportDto;

using Microsoft.Extensions.Logging;

using ReportSender.FileSystemManager.Abstract;
using ReportSender.MailSenderLogic.Abstract;
using ReportSender.ReportDto;

namespace ReportSender;

public class ConcordiaReportRunner : IConcordiaReportRunner
{
    private static int reportNumber = 0;
    private readonly IFileSystemDocumentManager _systemDocumentManager;
    private readonly ILogger<ConcordiaReportRunner> _logger;
    private readonly IMailSender _mailSender;

    public ConcordiaReportRunner(IFileSystemDocumentManager systemDocumentManager, ILogger<ConcordiaReportRunner> logger, IMailSender mailSender)
    {
        _systemDocumentManager = systemDocumentManager;
        _logger = logger;
        _mailSender = mailSender;
    }

    public async Task Run(IEnumerable<ExperimentForReportDto> experiments, IEnumerable<ScientistForReportDto> scientists)
    {
        _logger.LogInformation("Initiating Report Generation...");
        string path;
        await Task.Run(() =>
        {
            path = _systemDocumentManager.CheckAndGenerateFileStructure();
            ConcordiaReportBuilder builder = new ConcordiaReportBuilder(experiments, scientists, reportNumber);
            reportNumber++;
            builder.Build().Build(path);
            _logger.LogInformation("Sending email");
            var result = _mailSender.SendEmail(reportNumber, path);
            if (result.Item1)
                _logger.LogInformation(result.Item2);
            else
                _logger.LogWarning(result.Item2);
        });
        _logger.LogInformation("Report generated.");

    }
}
