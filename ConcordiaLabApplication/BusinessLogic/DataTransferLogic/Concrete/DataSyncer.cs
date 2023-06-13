using BusinessLogic.DataTransferLogic.Abstract;
using BusinessLogic.Exceptions;

using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace BusinessLogic.DataTransferLogic.Concrete;

public class DataSyncer : IDataSyncer
{

    private readonly ILogger<DataSyncer> _logger;
    private readonly IExperimentDownloader _experimentDownloader;
    private readonly ICommentDownloader _commentDownloader;
    private readonly IUploader _uploader;

    public DataSyncer(ILogger<DataSyncer> logger, IExperimentDownloader experimentDownloader, ICommentDownloader commentDownloader, IUploader uploader)
    {
        _logger = logger;
        _experimentDownloader = experimentDownloader;
        _commentDownloader = commentDownloader;
        _uploader = uploader;
    }

    public async Task SynchronizeAsync()
    {
        try
        {
            _logger.LogInformation("Download experiments from Trello started...");
            var task1 = await _experimentDownloader.DownloadExperiments();

            _logger.LogInformation("Experiments' download was completed successfully. ");
            if (!task1.IsNullOrEmpty())
                _logger.LogInformation($"Added {task1!.Count()} new experiments. ");
            else
                _logger.LogInformation("No experiment were added");


        }
        catch (ScientistIdNotPresentOnDatabaseException ex)
        {
            _logger.LogWarning($"An exception was caught while downloading experiments: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Un unexpected Exception was caught while downloading experiments: {ex.Message}");
        }

        try
        {
            _logger.LogInformation("Download comments from Trello started...");
            var task2 = await _commentDownloader.DownloadComments();
            _logger.LogInformation("Comments' download was completed successfully. ");
            if (!task2.IsNullOrEmpty())
                _logger.LogInformation($"Added {task2!.Count()} new experiments. ");
            else
                _logger.LogInformation("No experiment were added");
        }
        catch (ExperimentNotPresentInLocalDatabaseException ex)
        {
            _logger.LogWarning($"An exception was caught while downloading comments: {ex.Message}");
        }
        catch (AddACommentFailedException ex)
        {
            _logger.LogWarning($"An exception was caught while downloading comments: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Un unexpected Exception was caught while downloading comments: {ex.Message}");
        }

        try
        {
            _logger.LogInformation("Upload to Trello started...");
            await _uploader.Upload();
            _logger.LogInformation("Uploaded completed successfully");
        }
        catch (UploadFailedException ex)
        {
            _logger.LogWarning($"An exception was caught while uploading local data: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"Un unexpected Exception was caught while uploading local data: {ex.Message}");
        }

    }
}
