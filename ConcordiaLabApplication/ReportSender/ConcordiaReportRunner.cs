using BusinessLogic.DTOs.ReportDto;

using ReportSender.ReportDto;

namespace ReportSender;

public static class ConcordiaReportRunner
{
    private static int reportNumber = 0;
    public static void Run(IEnumerable<ExperimentForReportDto> experiments, IEnumerable<ScientistForReportDto> scientists)
    {
        Directory.GetCurrentDirectory();
        var path = Path.Combine(Directory.GetCurrentDirectory(), "report.pdf");
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(path);
        Console.ResetColor();
        ConcordiaReportBuilder builder = new ConcordiaReportBuilder(experiments, scientists, reportNumber);
        reportNumber++;
        builder.Build().Build(path);
    }
}
