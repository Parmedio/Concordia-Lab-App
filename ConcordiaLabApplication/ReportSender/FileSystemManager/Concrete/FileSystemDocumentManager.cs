using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using ReportSender.FileSystemManager.Abstract;

namespace ReportSender.FileSystemManager.Concrete;

public class FileSystemDocumentManager : IFileSystemDocumentManager
{
    private readonly string? ParentRoot;
    private readonly ILogger<FileSystemDocumentManager> _logger;
    private readonly IConfiguration _configuration;

    public FileSystemDocumentManager(IConfiguration configuration, ILogger<FileSystemDocumentManager> logger)
    {
        _configuration = configuration;
        _logger = logger;
        string configurationPath = _configuration.GetSection("ReportDirectoryPath").Value ?? "";
        try
        {
            if (configurationPath == "" || !Directory.Exists(configurationPath))
            {
                if (Directory.Exists(Directory.GetParent(Directory.GetCurrentDirectory())?.FullName))
                    ParentRoot = Directory.GetParent(Directory.GetCurrentDirectory())?.FullName;
                else
                    ParentRoot = Directory.GetCurrentDirectory();
            }
            else
                ParentRoot = configurationPath;
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Found an Exception while creating the file system structure. {Environment.NewLine}Exception: {ex.Message}{Environment.NewLine}{ex.InnerException?.Message ?? ""}");
            ParentRoot = Directory.GetCurrentDirectory();
        }
    }

    (string path, int count) IFileSystemDocumentManager.CheckAndGenerateFileStructure()
    {
        DirectoryInfo reportsDirectory = GetWorkingDirectory();
        int fileCount = reportsDirectory.EnumerateFiles().Count();
        return (Path.Combine(reportsDirectory.FullName, $"Report_{(fileCount + 1).ToString()}.pdf"), fileCount);
    }

    private DirectoryInfo GetWorkingDirectory()
    {
        if (!Directory.Exists(Path.Combine(ParentRoot!, "Reports")))
            return Directory.CreateDirectory(Path.Combine(ParentRoot!, "Reports"));
        return new DirectoryInfo(Path.Combine(ParentRoot!, "Reports"));
    }
}
