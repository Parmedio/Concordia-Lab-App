using BusinessLogic.DTOs.ReportDto;

using Microsoft.Extensions.Logging;

using ReportSender.FileSystemManager.Abstract;
using ReportSender.MailSenderLogic.Abstract;
using ReportSender.ReportDto;

namespace ReportSender;

public class ConcordiaReportRunner : IConcordiaReportRunner
{
    private readonly IFileSystemDocumentManager _systemDocumentManager;
    private readonly ILogger<ConcordiaReportRunner> _logger;
    private readonly IMailSender _mailSender;

    public ConcordiaReportRunner(IFileSystemDocumentManager systemDocumentManager, ILogger<ConcordiaReportRunner> logger, IMailSender mailSender)
    {
        _systemDocumentManager = systemDocumentManager;
        _logger = logger;
        _mailSender = mailSender;
    }

    public async Task<string> Run(IEnumerable<ExperimentForReportDto> experiments, IEnumerable<ScientistForReportDto> scientists, bool sendMail, DateTime currentDate)
    {

        _logger.LogInformation("Initiating Report Generation...");
        string path = String.Empty;
        await Task.Run(() =>
        {
            var fileInformations = _systemDocumentManager.CheckAndGenerateFileStructure();
            path = fileInformations.path;
            string reportId = $"{currentDate.Year}{currentDate.Day}{currentDate.Month}{fileInformations.count}";
            ConcordiaReportBuilder builder = new ConcordiaReportBuilder(experiments, scientists, reportId, currentDate);
            builder.Build().Build(path);

            if (sendMail)
            {
                _logger.LogInformation("Sending email");
                var result = _mailSender.SendEmail(reportId, path);
                if (result.Item1)
                    _logger.LogInformation(result.Item2);
                else
                    _logger.LogWarning(result.Item2);
            }
        });
        _logger.LogInformation("Report generated.");

        return path;
    }
}
