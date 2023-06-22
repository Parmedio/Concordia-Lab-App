namespace ReportSender.FileSystemManager.Abstract;

public interface IFileSystemDocumentManager
{
    internal (string path, int count) CheckAndGenerateFileStructure();
}
