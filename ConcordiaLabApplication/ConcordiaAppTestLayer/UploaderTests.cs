using BusinessLogic.DataTransferLogic.Abstract;
using BusinessLogic.DataTransferLogic.Concrete;
using BusinessLogic.Exceptions;

using ConcordiaAppTestLayer.MockData;

using FluentAssertions;

using Moq;

using PersistentLayer.Models;
using PersistentLayer.Repositories.Abstract;

namespace ConcordiaAppTestLayer;

public class UploaderTests
{
    private readonly IUploader _uploader;
    private readonly IApiSender _sender;
    private readonly IExperimentRepository _repository;

    public UploaderTests()
    {
        _sender = Mock.Of<IApiSender>();
        _repository = Mock.Of<IExperimentRepository>();
        _uploader = new Uploader(_sender, _repository);
    }

    [Fact]
    public void UploadShouldWork()
    {
        var data = new DataSyncerMockData2();
        Mock.Get(_repository).Setup(p => p.GetAll()).Returns(new List<Experiment>() { data.LocalExperiment1InToDo });
        Mock.Get(_sender).Setup(p => p.UpdateAnExperiment(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
        Mock.Get(_sender).Setup(p => p.AddAComment(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
        _uploader.Invoking(p => p.Upload()).Should().NotThrowAsync();
    }

    [Fact]
    public void UploadShouldThrowUploadFailedExpectionWhileAddingExperiments()
    {
        var data = new DataSyncerMockData2();
        Mock.Get(_repository).Setup(p => p.GetAll()).Returns(new List<Experiment>() { data.LocalExperiment1InToDo });
        Mock.Get(_sender).Setup(p => p.UpdateAnExperiment(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);
        Mock.Get(_sender).Setup(p => p.AddAComment(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
        _uploader.Invoking(p => p.Upload()).Should().ThrowAsync<UploadFailedException>().WithMessage($"The process failed while uploading experiments. Failed at experiment: {data.LocalExperiment1InToDo.Title}");
    }

    [Fact]
    public void UploadShouldThrowUploadFailedExceptionWhileAddingComments()
    {
        var data = new DataSyncerMockData2();
        Mock.Get(_repository).Setup(p => p.GetAll()).Returns(new List<Experiment>() { data.LocalExperiment1InToDo });
        Mock.Get(_sender).Setup(p => p.UpdateAnExperiment(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
        Mock.Get(_sender).Setup(p => p.AddAComment(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);
        _uploader.Invoking(p => p.Upload()).Should().ThrowAsync<UploadFailedException>().WithMessage($"The process failed while uploading the experiment: {data.LocalExperiment1InToDo.Title}. Error while trying to upload its latest comment: primoCommentoVecchio");
    }
}
